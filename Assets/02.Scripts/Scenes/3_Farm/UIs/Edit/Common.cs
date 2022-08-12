using System.Collections;
using UnityEngine;
using System;

public class Common : BuildingAttrib
{
    public string remainTimeStr;
    public GameObject goldBtn;
    public PopupBuilding popupBuilding;

    private int buildingId;
    private Coroutine m_CycleTimerCoroutine = null;

    [SerializeField] private AudioSource audioSource;
    ItemManager IM;
    
    private void Start()    // data.isDone 하지 않고 remainCycleTime==0 인 것은 첫 사이클이란 의미이므로 사이클 시간을 채워놔야 함
    {
        IM = ItemManager.Instance;

        // buildingId = data.id;
        // data = BuildingManager.Instance.GetData(buildingId);

        if (goldBtn == null) goldBtn = transform.GetChild(0).gameObject;
        // if (!data.isDone && data.cycleRemainTime == 0) data.cycleRemainTime = data.info.cycleTime;
        // if (!data.isDone) goldBtn.SetActive(false);
        
        LoadAppQuitTime();
        SetRechargeScheduler();    
    }

    public void SetRechargeScheduler()
    {
        if (m_CycleTimerCoroutine != null)
        {        
            StopCoroutine(m_CycleTimerCoroutine);
            m_CycleTimerCoroutine = null;
        }

        // var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_AppQuitTime).TotalSeconds); // 방치한 동안 흐른 시간(초) 계산
        // var remainTime = data.cycleRemainTime - timeDifferenceInSec;
        
        // if (remainTime <= 0)
        // {
        //     data.cycleRemainTime = 0;
        //     data.isDone = true;
        //     if (goldBtn != null) goldBtn.gameObject.SetActive(true);
        //     remainTimeStr = "남은 시간 : " + (data.cycleRemainTime / 60) + " 분 " + (data.cycleRemainTime % 60) + " 초";
        // } 
        // else
        // {
        //     m_CycleTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime));
        // }
    }

    private IEnumerator DoRechargeTimer(int remainTime)
    {
        WaitForSeconds sec = new WaitForSeconds(1f);

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
        if (IM.AddData((int)DataTable.Money, data.info.outputAmount) == false)
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

    public void ButtonUp()
    {
        // if (!isPointerDown)  // 이동이 아니라 골드 획득 혹은 팝업 노출
		{
			if (goldBtn.activeSelf) // 일반 건물, 작업 완료 시 골드 획득
				OnClickGoldBtn();
			else
			{
				popupBuilding.common = this;
				popupBuilding.gameObject.SetActive(true);
				popupBuilding.RenewPanel(popupBuilding.index);	
			}
		}
    }
}
