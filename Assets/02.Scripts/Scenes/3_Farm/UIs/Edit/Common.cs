using System.Collections;
using UnityEngine;
using System;

public class Common : MonoBehaviour
{
    public BuildingData data;
    public int buildingId;
    public int cycleOutput;                       // 단위시간별 골드 생산량
    private DateTime m_AppQuitTime;    // 유저 게임 이탈 시간 변수
    private Coroutine m_CycleTimerCoroutine = null;

    public string remainTimeStr;
    public GameObject goldBtn;
    AudioSource audioSource;
    ItemManager IM;

    private void OnApplicationFocus(bool focusStatus) 
    {
        data = BuildingManager.Instance.GetData(buildingId);
    }
    
    private void Start()    // data.isDone 하지 않고 remainCycleTime==0 인 것은 첫 사이클이란 의미이므로 사이클 시간을 채워놔야 함
    {
        IM = ItemManager.Instance;
        audioSource = GetComponent<AudioSource>();
        buildingId = GetComponent<EditBuilding>().data.id;
        data = BuildingManager.Instance.GetData(buildingId);

        if (goldBtn == null) goldBtn = transform.GetChild(0).gameObject;
        if (!data.isDone && data.cycleRemainTime == 0) data.cycleRemainTime = data.info.cycleTime;
        if (!data.isDone) goldBtn.SetActive(false);
        
        LoadAppQuitTime();
        SetRechargeScheduler();    
    }

    public bool LoadAppQuitTime() 
    { 
        bool result = false; 
        try 
        { 
            if (PlayerPrefs.HasKey("AppQuitTime")) 
            { 
                var appQuitTime = string.Empty; 
                appQuitTime = PlayerPrefs.GetString("AppQuitTime"); 
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime));
            }
            else
            { 
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(DateTime.Now.ToLocalTime().ToBinary().ToString())); 
            }
            result = true; 
        } 
        catch (System.Exception e) 
        { 
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")"); 
        } 
        return result; 
    }

    public void SetRechargeScheduler()
    {
        if (m_CycleTimerCoroutine != null)
        {        
            StopCoroutine(m_CycleTimerCoroutine);
        }

        var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_AppQuitTime).TotalSeconds); // 방치한 동안 흐른 시간(초) 계산
        var remainTime = data.cycleRemainTime - timeDifferenceInSec;
        
        if (remainTime <= 0)
        {
            data.cycleRemainTime = 0;
            data.isDone = true;
            if (goldBtn != null) goldBtn.gameObject.SetActive(true);
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime / 60) + " 분 " + (data.cycleRemainTime % 60) + " 초";
        } 
        else
        {
            m_CycleTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime));
        }
    }

    private IEnumerator DoRechargeTimer(int remainTime)
    {
        var sec = new WaitForSeconds(1f);

        if (remainTime <= 0)
        {
            data.cycleRemainTime = 0;
            data.isDone = true;
            if (goldBtn != null) goldBtn.gameObject.SetActive(true);
        }
        else // remainTime > 0
        {
            data.cycleRemainTime = remainTime;
        }
      
        while (data.cycleRemainTime > 0)
        {
            data.cycleRemainTime -= 1;
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime / 60) + " 분 " + (data.cycleRemainTime % 60) + " 초";
            yield return sec;
        }

        if(data.cycleRemainTime <= 0)
        {
            // 골드 생산 버튼 UI 띄우기
            data.cycleRemainTime = 0;
            data.isDone = true;
            if (goldBtn != null) goldBtn.gameObject.SetActive(true);
            m_CycleTimerCoroutine = null;
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime / 60) + " 분 " + (data.cycleRemainTime % 60) + " 초";
        }
    }

    public void OnClickGoldBtn()
    {
        if (IM.AddData((int)DataTable.Money, cycleOutput) == false)
            return;
        data.isDone = false;
        goldBtn.gameObject.SetActive(false);
        audioSource.Play();

        data.cycleRemainTime = data.info.cycleTime;
        if (m_CycleTimerCoroutine != null)
        {        
            StopCoroutine(m_CycleTimerCoroutine);
        }
        m_CycleTimerCoroutine = StartCoroutine(DoRechargeTimer(data.cycleRemainTime));
    }
}
