using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FishArea : MonoBehaviour, IPointerDownHandler
{
    // 양식장 영역에 삽입하는 스크립트
    private int fishCount;
    [SerializeField]
    private GameObject fishList;
    [SerializeField]
    private GameObject contents;
    private OpenCountUI[] spawnFishList;

    Vector3 gamePosition;
    private bool isSelected = false;
    public Color originColor;

    [SerializeField]
    private Text minText;
    [SerializeField]
    private Text secText;
    [SerializeField]
    private Text divideText;

    private bool canGrow = false;
    // private int minSec=61;
    private int beforeMinSec=10000000;

    [SerializeField] private AudioSource gatherFish;
    [SerializeField] private AudioClip fish_grow;
    [SerializeField] private AudioClip[] water = new AudioClip[2];

    ItemManager IM;

    [SerializeField]
    private GameObject fishInfomationPopup;
    private FishInfomationText fishInfomationText;
    private int growingFishKind;        //자라고 있는 물고기 종류

    void Start(){
        IM = ItemManager.Instance;

        gamePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        originColor = gameObject.GetComponent<SpriteRenderer>().color;
        spawnFishList = contents.GetComponentsInChildren<OpenCountUI>();
        fishInfomationText = gameObject.GetComponent<FishInfomationText>();
    }

    void Update(){
        //turnToOrigiin을 Update말고 다른 쪽에서 하는 방법이 없을까??
        if(spawnFishList[0].fishFarm_Be_Spawn != gameObject){     //즉 터치된게 아니라면
            turnToOrigin();
        }

        //로드한 뒤니까 Start에 옮겨도 될까?? 나중에 여유가 될때 하자
        if(gameObject.transform.childCount<3){       
            minText.text = "0";
            secText.text = "00";
        }


    }

    public void OnPointerDown(PointerEventData eventData)
    {
        fishCount = gameObject.transform.childCount;

        if(fishCount >= 3 && canGrow == false){     //물고기 O, 다 자라진 않음. 정보 팝업 출력해야 함
            //나중에 창으로 띄워야함.
            int ran = Random.Range(0, 2);
            gatherFish.PlayOneShot(water[ran]);
            //팝업 활성화
            fishInfomationPopup.SetActive(true);
            //팝업 정보 입력
            fishInfomationText.GetFishInfomation(growingFishKind);

        }else if(fishCount >=3 && canGrow == true)  //물고기 O, 다 자람.
        {
            //물고기에서 nextPrefab의 여부에 따라서 두개로 나누면 될것 같음.
            FishGrowTime[] nowFish = gameObject.GetComponentsInChildren<FishGrowTime>();
            GameObject nextFishPrefab = nowFish[0].nextFish;
            if(nextFishPrefab == null){ //2단계, 수확
                for(int i=0; i<fishCount -2; i++){
                    if (IM.AddData(nowFish[i].fishKind + (int)DataTable.Fish + 1) == false)
                        return;
                    nowFish[i].DeleteThisGameObject();
                }
                gatherFish.Play();
            }else{//클릭 시 알에서 물고기로 진화
                gatherFish.PlayOneShot(fish_grow);
                for(int i=0; i<fishCount - 2; i++){
                    Instantiate(nextFishPrefab, nowFish[i].GetPosition(), Quaternion.identity);
                    nowFish[i].DeleteThisGameObject();
                }
            }
        }
        else       //물고기 X
        {            
            if(isSelected == false){      //물고기가 심어진 상태가 아닐때 양식장이 선택되면 물고기 넣음
                gameObject.GetComponent<SpriteRenderer>().color =  new Color(1f, 1f, 0); // -> 색깔 초록색으로 / 삭제시 FishFarmColor도 삭제
                gatherFish.PlayOneShot(water[0]);
                fishList.SetActive(true);
                for(int i=0; i<spawnFishList.Length; i++){  // 물고기를 넣을 양식장 선택 - 모든 물고기 알 버튼에 대해...
                    spawnFishList[i].SetFishFarm(gameObject);
                }
                isSelected = true;                          // 이 양식장은 선택됐어. 물고기가 들어있어.
            }
        }
    }

    public void turnToOrigin(){
        gameObject.GetComponent<SpriteRenderer>().color = originColor;
        isSelected = false;
    }

    public void TimeTextUpdate(int min, int sec, int fishKind){
        //Debug.Log("minSec: "+ minSec);
        int areaMinSec = min*60 + sec;
        growingFishKind = fishKind;     //이 Area에 대해 자라고 있는 물고기 값이 들어간다.

        if(beforeMinSec > areaMinSec){
            beforeMinSec = areaMinSec;
            minText.text = min.ToString();
            secText.text = sec.ToString();
            if(min == 0 && sec == 0){       //시간이 0이면
                beforeMinSec = 10000000;
                canGrow = true;
            }else{
                canGrow = false;
            }
        }
    }
}
