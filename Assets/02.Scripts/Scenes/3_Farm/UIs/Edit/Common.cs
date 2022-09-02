using System.Collections;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Common : BuildingAttrib, IPointerUpHandler
{
    public string remainTimeStr;
    public PopupBuilding popupBuilding;
    [SerializeField] Building building;
    [SerializeField] GameObject goldBtn;
    [SerializeField] AudioSource audioSource;

    Coroutine m_CycleTimerCoroutine = null;
    
    private void Start()
    {
        buildingId = data.id;
        data = BuildingManager.Instance.GetData(buildingId);
        TouchManager.ZoomAmountChange += this.IconSizeChange;

        if (!data.isDone && data.cycleRemainTime == 0) // first cycle
            data.cycleRemainTime = data.info.cycleTime;
        if (!data.isDone) 
            goldBtn.SetActive(false);
        
        LoadAppQuitTime();
        SetRechargeScheduler();    
    }

    public void IconSizeChange(float zoomAmount)
    {
        goldBtn.transform.localScale = Vector3.one * zoomAmount * 0.2f;
    }

    private void OnDestroy() 
    {
        TouchManager.ZoomAmountChange -= this.IconSizeChange;
    }

    override public void SetRechargeScheduler(bool newCycle=false)
    {
        if (m_CycleTimerCoroutine != null)
        {        
            StopCoroutine(m_CycleTimerCoroutine);
            m_CycleTimerCoroutine = null;
        }

        if (newCycle == true)
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
            goldBtn.gameObject.SetActive(true);
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime).Sec2Time();
        } 
        else
            m_CycleTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime));
    }

    private IEnumerator DoRechargeTimer(int remainTime)
    {
        WaitForSeconds sec = new WaitForSeconds(1f);

        if (remainTime <= 0)
        {
            data.cycleRemainTime = 0;
            data.isDone = true;
            goldBtn.gameObject.SetActive(true);
        }
        else // remainTime > 0
        {
            data.cycleRemainTime = remainTime;
        }
      
        while (data.cycleRemainTime > 0)
        {
            data.cycleRemainTime -= 1;
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime).Sec2Time();
            yield return sec;
        }

        if(data.cycleRemainTime <= 0)
        {
            // 골드 생산 버튼 UI 띄우기
            data.cycleRemainTime = 0;
            data.isDone = true;
            goldBtn.gameObject.SetActive(true);
            m_CycleTimerCoroutine = null;
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime).Sec2Time();
        }
    }

    public void OnClickGoldBtn()
    {
        audioSource.Play();
        if (ItemManager.Instance.AddData((int)DataTable.Money, data.info.outputAmount) == false)
            return;
        data.isDone = false;
        goldBtn.gameObject.SetActive(false);
        
        SetRechargeScheduler(true);
    }

    public void OnPointerUp(PointerEventData e)
    {
        if (!building.isPointerDown)  // 이동이 아니라 골드 획득 혹은 팝업 노출
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
