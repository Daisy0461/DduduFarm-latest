using System.Collections;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Craft : BuildingAttrib, IPointerClickHandler
{
    public DduduSpawner DS;
    public string remainTimeStr;
    public PopupBuilding popupBuilding;
    [SerializeField] Building building;
    [SerializeField] GameObject outputBtn;
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
            outputBtn.SetActive(false);
        
        LoadAppQuitTime();
        SetRechargeScheduler();
    }

    public void IconSizeChange(float zoomAmount)
    {
        outputBtn.transform.localScale = Vector3.one * zoomAmount * 0.2f;
    }

    private void OnDestroy() 
    {
        TouchManager.ZoomAmountChange -= this.IconSizeChange;
    }


    override public void SetRechargeScheduler(bool newCycle=false)
    {
        if (!IsWorking()) return;

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
            outputBtn.gameObject.SetActive(true);
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime).Sec2Time();
            DduduOut();
        } 
        else
        {
            m_CycleTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime));
            ExpansionAnimation();
        }
    }

    private IEnumerator DoRechargeTimer(int remainTime)
    {
        WaitForSeconds sec = new WaitForSeconds(1f);

        if (remainTime <= 0)
        {
            data.cycleRemainTime = 0;
            data.isDone = true;
            outputBtn.gameObject.SetActive(true);
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
            // 생산 버튼 UI 띄우기
            data.cycleRemainTime = 0;
            data.isDone = true;
            outputBtn.gameObject.SetActive(true);
            m_CycleTimerCoroutine = null;
            remainTimeStr = "남은 시간 : " + (data.cycleRemainTime).Sec2Time();
            DduduOut();
        }
    }

    public void OnClickOutput()
    {
        audioSource.Play();
        if (ItemManager.Instance.AddData(data.info.outputId, data.info.outputAmount) == false) 
            return;
        data.isDone = false;
        outputBtn.SetActive(false);
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (!building.isPointerDown)  // 이동이 아니라 골드 획득 혹은 팝업 노출
		{
            if (outputBtn.activeSelf)   // 공방 건물, 작업 완료 시 보상 획득
			    OnClickOutput();
            else
			{
				popupBuilding.craft = this;
				popupBuilding.gameObject.SetActive(true);
				popupBuilding.RenewPanel(popupBuilding.index);	
			}
        }
    }

    public bool IsWorking()
    {
        bool ret = false;
        // data.isDone = transform.GetChild(0).gameObject.activeSelf;
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
}
