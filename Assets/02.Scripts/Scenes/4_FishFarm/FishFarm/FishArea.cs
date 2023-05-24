using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FishArea : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject fishList;
    [SerializeField] private GameObject contents;
    [SerializeField] private Text minText;
    [SerializeField] private Text secText;
    [SerializeField] private AudioSource gatherFish;
    [SerializeField] private AudioClip fish_grow;
    [SerializeField] private AudioClip[] water = new AudioClip[2];
    [SerializeField] private GameObject fishInfomationPopup;
    
    private Color originColor;
    private OpenCountUI[] spawnFishList;
    private Vector3 gamePosition;
    private FishInfomationText fishInfomationText;
    private bool isSelected = false;
    private bool canGrow = false;
    private int fishCount;
    private int beforeMinSec = 10000000;
    private int growingFishKind;        //자라고 있는 물고기 종류

    void Start()
    {
        gamePosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        originColor = gameObject.GetComponent<SpriteRenderer>().color;
        spawnFishList = contents.GetComponentsInChildren<OpenCountUI>();
        fishInfomationText = gameObject.GetComponent<FishInfomationText>();
        SetTextEmptyFishFarm();
    }

    void Update()
    {
        if (spawnFishList[0].fishFarm_Be_Spawn != gameObject)
        {     //즉 터치된게 아니라면
            TurnToOrigin();
        }        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // TOOD: GetComponent로 물고기 수 세는 것을 리스트 Count로 바꿔야함
        fishCount = gameObject.transform.GetComponentsInChildren<FishGrowTime>().Length;

        if (fishCount > 0)
        {
            if (canGrow == false) //물고기 O, 다 자라진 않음. 정보 팝업 출력해야 함
            {     
                int ran = Random.Range(0, 2);
                gatherFish.PlayOneShot(water[ran]);
                fishInfomationPopup.SetActive(true);
                fishInfomationText.GetFishInfomation(growingFishKind);
            }
            else if (canGrow == true)  //물고기 O, 다 자람.
            {
                FishGrowTime[] nowFish = gameObject.GetComponentsInChildren<FishGrowTime>();
                GameObject nextFishPrefab = nowFish[0].nextFish;
                if (nextFishPrefab == null) //2단계, 수확
                { 
                    for (int i=0; i < fishCount - 2; i++)
                    {
                        if (ItemManager.Instance.AddData(nowFish[i].fishKind + (int)DataTable.Fish + 1) == false) return;
                        nowFish[i].DeleteThisGameObject();
                    }
                    gatherFish.Play();
                    SetTextEmptyFishFarm();
                }
                else //클릭 시 알에서 물고기로 진화
                {
                    gatherFish.PlayOneShot(fish_grow);
                    for (int i = 0; i < fishCount - 2; i++)
                    {
                        Instantiate(nextFishPrefab, nowFish[i].GetPosition(), Quaternion.identity);
                        nowFish[i].DeleteThisGameObject();
                    }
                }
            }
        }
        else       //물고기 심기
        {            
            if (isSelected == false)
            {      
                gameObject.GetComponent<SpriteRenderer>().color =  new Color(1f, 1f, 0); // -> 색깔 초록색으로 / 삭제시 FishFarmColor도 삭제
                gatherFish.PlayOneShot(water[0]);
                fishList.SetActive(true);
                for (int i = 0; i < spawnFishList.Length; i++) 
                {  // 물고기를 넣을 양식장 선택 - 모든 물고기 알 버튼에 대해...
                    spawnFishList[i].SetFishFarm(gameObject);
                }
                isSelected = true;
            }
        }
    }

    private void SetTextEmptyFishFarm()
    {       
        minText.text = "0";
        secText.text = "00";
    }

    public void TurnToOrigin()
    {
        gameObject.GetComponent<SpriteRenderer>().color = originColor;
        isSelected = false;
    }

    public void TimeTextUpdate(int min, int sec, int fishKind)
    {
        int areaMinSec = min*60 + sec;
        growingFishKind = fishKind;     //이 Area에 대해 자라고 있는 물고기 값이 들어간다.

        if (beforeMinSec > areaMinSec)
        {
            beforeMinSec = areaMinSec;
            minText.text = min.ToString();
            secText.text = sec.ToString();
            if (min == 0 && sec == 0)
            {       //시간이 0이면
                beforeMinSec = 10000000;
                canGrow = true;
            }
            else
            {
                canGrow = false;
            }
        }
    }
}
