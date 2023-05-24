using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;
using System;


public class FishGrowTime : MonoBehaviour
{
    //fish 종류
    [Header("물고기 종류와 레벨")] [Space(10f)] [Tooltip("물고기 종류 - 0부터 시작")]
    public int fishKind;
    
    [Tooltip("물고기 레벨 - 0부터 시작")]
    [SerializeField] private int fishLevel;
    
    [Space(10f)]
    public GameObject nextFish;
    
    [SerializeField] private GameObject lastFish;
    [SerializeField] private int timeMaxGrowInterval = 60; //다 자르는데 걸리는 시간. 
    
    public Vector3 originPos;
    public int remainGrowTime = 60;       
    
    private DateTime m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
    private Coroutine m_RechargeTimerCoroutine = null;
    private DateTime check_m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();      //체크용 시간. 이거 없으면 이상한 버그 발생함.
    private Vector3 fishPos;
    private GameObject fishTime;
    private FishArea fishArea;
    private bool set_check = false;
    
    void Awake()
    {
        originPos = gameObject.GetComponent<Transform>().position;
    }

    void OnApplicationFocus(bool value)
    {
        if (value)
        {
            set_check = true;
        }
    }

    void Start()
    {
        fishTime = GameObject.FindWithTag("SaveManager"); 
        m_AppQuitTime = fishTime.GetComponent<FishTime>().get_m_AppQuitTime();      //나간 시간 로드하는 건데
        fishPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        set_check = true;

        var researchEffeect = PlayerPrefs.GetFloat("성장 가속", 1);
        var fishGrowTime = FishManager.Instance.GetInfo((int)DataTable.Fish + fishKind + 1).grow1Time;
        timeMaxGrowInterval = (int)(fishGrowTime * researchEffeect);
    }

    void Update()
    {
        if(set_check)
        {
            SetRechargeScheduler();
            set_check = false;
        }
    }

    public void SetRechargeScheduler(Action onFinish = null)
    {
        if(m_AppQuitTime == check_m_AppQuitTime)
        {       //여기 같으면 return하도록 했음 그냥.
            m_AppQuitTime = DateTime.Now.ToLocalTime();
        }

        if (m_RechargeTimerCoroutine != null)
        {       
            StopCoroutine(m_RechargeTimerCoroutine);
        }

        m_AppQuitTime = fishTime.GetComponent<FishTime>().get_m_AppQuitTime();
        
        if(remainGrowTime != timeMaxGrowInterval)
        {  
            var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_AppQuitTime).TotalSeconds);    //지금 시간이랑 앱을 껐을 떄의 시간을 뺀 값이 들어가게 된다.
            var remainTime = remainGrowTime - timeDifferenceInSec;  
            if (remainTime <= timeMaxGrowInterval)      
            {
                m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
            }
        }else       //remainGrowTime == timeMaxGrowInterval로 새로 생겼다는 의미
        {
            var remainTime = remainGrowTime;
            if (remainTime <= timeMaxGrowInterval)      
            {
                m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
            }
        }
    }

    private IEnumerator DoRechargeTimer(int remainTime, Action onFinish = null)
    {
        while (fishArea == null)
        {        //fishArea가 인지가 안되면 시간이 안넘어감
            yield return null;
        }

        bool isInstantiated = false;
        if (remainTime <= 0)            //즉 남은시간이 없다. 다 자랐다.
        {
            isInstantiated = true;
            remainGrowTime = 0;
            if (lastFish != null)
            {
                Instantiate(lastFish, fishPos, Quaternion.identity);
                Destroy(gameObject);
            }
            fishArea.TimeTextUpdate(0, 0, fishKind);
        }
        else //remainTime > 0
        {
            remainGrowTime = remainTime;
        }

        while (remainGrowTime > 0)        //다 자랄때까지 시간이 남았을 때 돌리는 while이고 1초씩 기다리면서 1f씩 줄인다. 
        {
            remainGrowTime -= 1;
            int remainGrowTimeMinute = remainGrowTime/60;
            int remainGrowTimeSec = remainGrowTime - remainGrowTimeMinute*60;

            fishArea.TimeTextUpdate(remainGrowTimeMinute, remainGrowTimeSec, fishKind);

            yield return new WaitForSeconds(1f);        //결국 마지막에는 remainGrowTime = 0이 된다.
        }

        if(remainGrowTime <= 0 && isInstantiated == false){        //다 자람
            if(lastFish != null){
                Instantiate(lastFish, fishPos, Quaternion.identity);
                Destroy(gameObject);
            }
            fishArea.TimeTextUpdate(0, 0, fishKind);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {       //꼭 움직여야 반응함! //Trigger  Collision
        if(col.gameObject.tag == "Farm")
        {
            gameObject.transform.parent = col.gameObject.transform;
            fishArea = gameObject.GetComponentInParent<FishArea>();
        }
    }

    public void DeleteThisGameObject()
    {
        Destroy(gameObject);
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
}
