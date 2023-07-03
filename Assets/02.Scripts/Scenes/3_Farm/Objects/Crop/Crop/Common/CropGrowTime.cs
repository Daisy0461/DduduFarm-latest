using System.Collections;
using UnityEngine;
using System;

//이게 실제 작물에 넣을 코드임.

public class CropGrowTime : MonoBehaviour
{
    public int remainGrowTime = -1;
    public int cropKind;    // info code
    public int cropLevel;   // 0, 1
    public Vector3 originPos;
    
    private DateTime _m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
    
    private CropState _cropState;
    private LastCropState _lastCropState;
    private Coroutine _m_RechargeTimerCoroutine = null;
    private WaitForSeconds _oneSecond = new WaitForSeconds(1f);

    void Awake()
    {
        _cropState = GetComponent<CropState>();
        _lastCropState = GetComponent<LastCropState>();
        originPos = gameObject.GetComponent<Transform>().position;
    }

    public int GetTimeMaxGrowInterval()
    {
        var growTime = 0;
        switch (cropLevel)
        {
            case 0 :
            {
                growTime = CropManager.Instance.GetInfo(cropKind).grow1Time;
                break;
            }
            case 1 :
            {
                growTime = CropManager.Instance.GetInfo(cropKind).grow2Time;
                break;
            }
        }

        var cal = PlayerPrefs.GetFloat("토질 연구", 1);
        growTime = (int)(growTime * cal);
        return growTime;
    }

    public void SetRechargeScheduler(bool newCycle)
    {
        if (_m_RechargeTimerCoroutine != null)
        {        
            StopCoroutine(_m_RechargeTimerCoroutine);
            _m_RechargeTimerCoroutine = null;
        }

        if (newCycle)
        {
            remainGrowTime = GetTimeMaxGrowInterval();
            _m_AppQuitTime = DateTime.Now.ToLocalTime();
        }
        else 
        {
            SaveManager.TryLoadAppQuitTime(out _m_AppQuitTime);
        }

        var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - _m_AppQuitTime).TotalSeconds);
        remainGrowTime = remainGrowTime - timeDifferenceInSec;

        if(remainGrowTime <= 0)
        {     
            GrowDoneAction();
            return;
        }
        _m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer());
    }

    private IEnumerator DoRechargeTimer()
    {
        while (remainGrowTime > 0)
        {
            remainGrowTime -= 1;
            yield return _oneSecond;
        }
        
        GrowDoneAction();
    }

    private void GrowDoneAction()
    {
        if(cropLevel == 0 && _cropState != null)
        {
            _cropState.GrowDone();         
        }
        else if (cropLevel == 1 && _lastCropState != null)
        {
            _lastCropState.GrowDone();
        }
    }

    public int GetRemainGrowTime()
    {
        return remainGrowTime;
    }
}
