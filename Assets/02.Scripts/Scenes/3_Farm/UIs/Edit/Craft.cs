using System.Collections;
using UnityEngine;
using System;

public class Craft : BuildingAttrib
{
    public DduduSpawner DS;
    public BuildingData data;
    public int buildingId;
    public GameObject outputBtn;
    public string remainTimeStr;
    public PopupBuilding popupBuilding;
    private DateTime m_AppQuitTime;    // 유저 게임 이탈 시간 변수
    private Coroutine m_CycleTimerCoroutine = null;
    WaitForSeconds sec = new WaitForSeconds(1f);
    [SerializeField] private AudioSource audioSource;

    private void OnApplicationFocus(bool focusStatus) 
    {
        var tmpBuilding = BuildingManager.Instance.GetData(buildingId);
        tmpBuilding = data;
        if (focusStatus == true && IsWorking()) SetRechargeScheduler();
    }
    
    private void Start() 
    {
        buildingId = data.id;
        data = BuildingManager.Instance.GetData(buildingId);

        if (outputBtn == null) outputBtn = transform.GetChild(0).gameObject;
        if (!data.isDone && data.cycleRemainTime == 0) data.cycleRemainTime = data.info.cycleTime;
        if (!data.isDone) outputBtn.SetActive(false);
        LoadAppQuitTime();
        if (IsWorking()) SetRechargeScheduler();
    }

    public bool LoadAppQuitTime() 
    { 
        bool result = false; 
        try 
        { 
            if (PlayerPrefs.HasKey("AppQuitTime")) 
            { 
                var appQuitTime = PlayerPrefs.GetString("AppQuitTime"); 
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime)); 
            }
            else 
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(DateTime.Now.ToLocalTime().ToBinary().ToString())); 
            result = true; 
        } 
        catch (System.Exception e) 
        {
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")"); 
        } 
        return result; 
    }

    public void SetRechargeScheduler(bool call=false)
    {

        if (m_CycleTimerCoroutine != null) 
        {
            StopCoroutine(m_CycleTimerCoroutine);
            m_CycleTimerCoroutine = null;
        }
        if (call == true) 
        {
            data.cycleRemainTime = data.info.cycleTime;
            m_AppQuitTime = DateTime.Now.ToLocalTime();
        }
        var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_AppQuitTime).TotalSeconds); // 방치한 동안 흐른 시간(초) 계산
        var remainTime = data.cycleRemainTime - timeDifferenceInSec;
        
        if (remainTime <= 0)
        {
            data.cycleRemainTime = 0;
            data.isDone = true;
            if (outputBtn != null) outputBtn.gameObject.SetActive(true);
            DduduOut();
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime / 60) + " 분 " + (data.cycleRemainTime % 60) + " 초";
        } 
        else
            m_CycleTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime));
    }

    private IEnumerator DoRechargeTimer(int remainTime)
    {
        data.cycleRemainTime = remainTime;
        while (data.cycleRemainTime > 0)
        {
            yield return sec;
            data.cycleRemainTime -= 1;
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime / 60) + " 분 " + (data.cycleRemainTime % 60) + " 초";
        }

        if(data.cycleRemainTime <= 0)
        {   // 생산 버튼 UI 띄우기
            data.cycleRemainTime = 0;
            data.isDone = true;
            BuildingManager.Instance.GetData(data.id).isDone = true;
            if (outputBtn != null) outputBtn.gameObject.SetActive(true);
            DduduOut();
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime / 60) + " 분 " + (data.cycleRemainTime % 60) + " 초";
            m_CycleTimerCoroutine = null;
        }
    }

    public void OnClickOutput()
    {
        audioSource.Play();
        if (ItemManager.Instance.AddData(data.info.outputId, data.info.outputAmount) == false) return;
        BuildingManager.Instance.GetData(data.id).isDone = false;
        data.isDone = false;
        outputBtn.SetActive(false);
    }

    public bool IsWorking()
    {
        bool ret = false;
        data.isDone = transform.GetChild(0).gameObject.activeSelf;
        if ((data.workerId != 0 && !data.isDone)      // 작업중 - data.worker 있고, isDone false
            || (data.workerId == 0 && data.isDone))  // 작업완료 - data.worker 없고, isDone true  
            ret = true;
        return ret;
    }

    public void DduduOut()
    {
        if (data.workerId == 0) return;
        DduduManager.Instance.GetData(data.workerId).satiety -= data.info.cycleTime * 2;
        DduduManager.Instance.GetData(data.workerId).isWork = false;
        var ddudu = DS.FindDduduObject(data.workerId).gameObject;
        ddudu.transform.position = new Vector3(transform.position.x, transform.position.y-1, transform.position.z);
        ddudu.SetActive(true);
        data.workerId = 0;
    }

    public void ButtonUp()
    {
        // if (!isPointerDown)  // 이동이 아니라 골드 획득 혹은 팝업 노출
		{
            if (outputBtn.activeSelf)
			    OnClickOutput();
        }
    }
}
