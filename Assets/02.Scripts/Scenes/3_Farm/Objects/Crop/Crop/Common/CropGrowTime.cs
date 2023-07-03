using System.Collections;
using UnityEngine;
using System;

//이게 실제 작물에 넣을 코드임.

public class CropGrowTime : MonoBehaviour
{
    [HideInInspector] public int remainGrowTime = 60;
    public int cropKind;    // info code
    public int cropLevel;   // 0, 1
    public Vector3 originPos;
    
    private int _timeMaxGrowInterval; // growNTime; 
    private DateTime _m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
    
    private CropState _cropState;
    private LastCropState _lastCropState;
    private Coroutine _m_RechargeTimerCoroutine = null;
    private DateTime _check_m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();      //체크용 시간. 이거 없으면 이상한 버그 발생함.
    private WaitForSeconds _oneSecond = new WaitForSeconds(1f);

    void Awake()
    {
        _cropState = GetComponent<CropState>();
        _lastCropState = GetComponent<LastCropState>();
        originPos = gameObject.GetComponent<Transform>().position;
    }

    void OnApplicationFocus(bool isFocus)
    {
        if (isFocus)
        {
            var cropTime = GameObject.FindObjectOfType<CropTime>(); 
            _m_AppQuitTime = cropTime.get_m_AppQuitTime();
            SetRechargeScheduler();          
        }
    }

    void Start()
    {
        remainGrowTime = GetTimeMaxGrowInterval();
        SetRechargeScheduler();
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
            default: 
            {
                Debug.LogError("crop Level is not in case");
                return 0;
            }
        }

        var cal = PlayerPrefs.GetFloat("토질 연구", 1);
        growTime = (int)(growTime * cal);
        return growTime;
    }

    public void SetRechargeScheduler(Action onFinish = null)
    {
        if(_m_AppQuitTime == _check_m_AppQuitTime)
        {
            _m_AppQuitTime = DateTime.Now.ToLocalTime();
        }

        if (_m_RechargeTimerCoroutine != null)
        {        
            StopCoroutine(_m_RechargeTimerCoroutine);
            _m_RechargeTimerCoroutine = null;
        }

        _timeMaxGrowInterval = GetTimeMaxGrowInterval();
        
        if(remainGrowTime < _timeMaxGrowInterval)
        {     
            var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - _m_AppQuitTime).TotalSeconds);    //지금 시간이랑 앱을 껐을 떄의 시간을 뺀 값이 들어가게 된다.
            Debug.Log($"timeDifferenceInSec : {timeDifferenceInSec}");
            remainGrowTime = remainGrowTime - timeDifferenceInSec;
        }
        _m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer());
    }

    private IEnumerator DoRechargeTimer()
    {
        Debug.Log($"remainGrowTime : {remainGrowTime}");
        while (remainGrowTime > 0)
        {
            remainGrowTime -= 1;
            yield return _oneSecond;
        }
        
        if(cropLevel == 0 && _cropState != null)
        {
            Debug.Log("growDone");
            _cropState.GrowDone();         
        }
        else if (cropLevel == 1 && _lastCropState != null)
        {
            _lastCropState.GrowDone();
        }
        else
        {
            Debug.Log("cropState or lastCropState를 들고오지 못함");
        }
    }

    public int GetRemainGrowTime()
    {
        return remainGrowTime;
    }
}
