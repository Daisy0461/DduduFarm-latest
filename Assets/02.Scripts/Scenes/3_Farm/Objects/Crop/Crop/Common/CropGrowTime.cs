using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//이게 실제 작물에 넣을 코드임.

public class CropGrowTime : MonoBehaviour
{
    private CropState cropState;
    private LastCropState lastCropState;

    private Coroutine m_RechargeTimerCoroutine = null;
    public int timeMaxGrowInterval = 60; //다 자르는데 걸리는 시간. 
    public int remainGrowTime = 60;       
    public DateTime m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
    private DateTime check_m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();      //체크용 시간. 이거 없으면 이상한 버그 발생함.
    private bool set_check = false;
    private GameObject cropTime;
    public Vector3 originPos;

    //crop 종류
    public int cropKind;
    public int cropLevel;

    private LabData labData;
    private int labDecrease;

    void Awake(){
        cropState = gameObject.GetComponent<CropState>();
        if(cropState == null){
            //Debug.Log("null로 찾아짐.");
            lastCropState = gameObject.GetComponent<LastCropState>();
        }
        originPos = gameObject.GetComponent<Transform>().position;
    }

    void OnApplicationFocus(bool value){
        if (value)
        {   
            set_check = true;           
        }
    }

    void Start(){
        cropTime = GameObject.FindWithTag("SaveManager"); 
        m_AppQuitTime = cropTime.GetComponent<CropTime>().get_m_AppQuitTime();

        SetRechargeScheduler();     //처음엔 여기서 시작.
        set_check = false;

        labData = FindObjectOfType<LabData>();
        labDecrease = checkCropKind();     //여기에 함수 만들어서 적용하면 될듯.
        double doubleTimeMax = timeMaxGrowInterval;
        double percent = Math.Ceiling((doubleTimeMax/100) * labDecrease);
        int cal = (int)percent;

        timeMaxGrowInterval = timeMaxGrowInterval - cal;
        remainGrowTime = remainGrowTime - cal;
    }

    void Update(){
        if(set_check){
            SetRechargeScheduler();
            set_check = false;
        }
    }

    public void SetRechargeScheduler(Action onFinish = null)
    {
        if(m_AppQuitTime == check_m_AppQuitTime){       //여기 같으면 return하도록 했음 그냥.
            m_AppQuitTime = DateTime.Now.ToLocalTime();
        }

        if (m_RechargeTimerCoroutine != null)
        {        
            StopCoroutine(m_RechargeTimerCoroutine);
        }

        m_AppQuitTime = cropTime.GetComponent<CropTime>().get_m_AppQuitTime();
        
        if(remainGrowTime != timeMaxGrowInterval){     

            var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_AppQuitTime).TotalSeconds);    //지금 시간이랑 앱을 껐을 떄의 시간을 뺀 값이 들어가게 된다.
            var remainTime = remainGrowTime - timeDifferenceInSec;  

            if (remainTime <= timeMaxGrowInterval)      
            {
                m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
            }
        }else       //remainGrowTime == timeMaxGrowInterval로 새로 생겼다는 의미
        {
            var remainTime = remainGrowTime;        //이게 들어가도 상관이 없는게 결국 Awake에서 timeMax로 조절을 해줘서 상관없음.
            if (remainTime <= timeMaxGrowInterval)      
            {
                m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
            }
        }
    }

    private IEnumerator DoRechargeTimer(int remainTime, Action onFinish = null)
    {
        //Debug.Log("DoRechargeTimer");
        if (remainTime <= 0)            //즉 남은시간이 없다. 다 자랐다.
        {
            remainGrowTime = 0;
        }
        else //remainTime > 0
        {
            remainGrowTime = remainTime;
        }
      
        while (remainGrowTime > 0)        //다 자랄때까지 시간이 남았을 때 돌리는 while이고 1초씩 기다리면서 1f씩 줄인다. 
        {
            //Debug.Log("CropGrowingTimer : " + remainGrowTime + "s");
            remainGrowTime -= 1;
            // int remainGrowTimeMinute = remainGrowTime/60;
            // int remainGrowTimeSec = remainGrowTime - remainGrowTimeMinute*60;
            // timeMinText.text = remainGrowTimeMinute.ToString();
            // timeSecText.text = remainGrowTime.ToString();
            yield return new WaitForSeconds(1f);        //결국 마지막에는 remainGrowTime = 0이 된다.
        }

        if(remainGrowTime <= 0){
            if(cropState != null){
                cropState.GrowDone();         
            }else if (lastCropState != null){
                lastCropState.GrowDone();
            }else{
                Debug.Log("cropState or lastCropState를 들고오지 못함");
            }
        }
    }

    public int getRemainGrowTime(){
        return remainGrowTime;
    }
    public int getTimeMaxGrowInterval(){
        return timeMaxGrowInterval;
    }

    private int checkCropKind(){
        if (labData == null) return 0;
        return labData.farmTimeDecrease;
    }
}
