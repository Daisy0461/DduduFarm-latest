using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmState : MonoBehaviour, IPointerDownHandler
{
    Vector3 farmPosition;
    private Color originColor;
    //[HideInInspector]
    public bool afterPushFunc = false;
    private bool isPlanted = false;
    [SerializeField]
    private bool isSelected = false;
    [SerializeField]
    private SpriteRenderer farmSpriteRanderer;
    [SerializeField]
    private SpriteRenderer childSpriteRanderer;

    [SerializeField] [Tooltip ("자기 부모의 FarmSelect를 넣으면 됌.")]
    private FarmSelect parentFarmSelect;
    ItemManager IM;

    [Header ("선택된 씨앗")]
    public GameObject CropKind;

    [HideInInspector]
    public CropSelectButton cropSelectButton;
    [SerializeField]
    private AudioSource plantSound;
    private SpriteRenderer SR;

    private bool plantMode = false;

    void Start()
    {
        IM = ItemManager.Instance;
        farmPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        originColor = farmSpriteRanderer.color;
    }

    void Update()
    {
        if(isSelected)
        {
            afterPushFunc = true;
        }
    }
 
    void OnCollisionEnter2D(Collision2D coll)   //작물이 들어올 때 작동
    {   // 밭의 네모난 것과 작물이 충돌
        if(coll.gameObject.tag == "Crop")
        {     
            isPlanted = true;
            if(plantMode == false)
            {             //작물이 들어왔는데 지금은 작물을 심고있는 중이 아니다.
                farmSpriteRanderer.color =  originColor;
                childSpriteRanderer.color = originColor;     
            }
            else
            {                              //작물이 들어왔으며 지금은 작물을 심고있는 중이다.
                farmSpriteRanderer.color =  new Color(255/255f ,85/255f, 85/255f, 255/255f);
                childSpriteRanderer.color = new Color(255/255f ,85/255f, 85/255f, 255/255f);
            }
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Crop")
        {
            isPlanted = false;
            farmSpriteRanderer.color =  new Color(255/255f, 230/255f, 128/255f, 255/255f);
            childSpriteRanderer.color = originColor;
        }

        farmSpriteRanderer.color = originColor;
        childSpriteRanderer.color = originColor;
        gameObject.SetActive(false);
        gameObject.SetActive(true);     //완결까지 채집을 하니 충돌이 없어서 색이 변하지 않는다.
        if(plantMode)
        {
            ChangeFarmColor();
        }
        else
        {
            HavastColorChange();
        }
    }

    public void ChangeFarmColor()
    {      //여기에 색 변화하게
        plantMode = true;
        if(!isPlanted)
        {     //작물이 심어져 있지 않은 상태라면 아래를 실행한다.
            farmSpriteRanderer.color =  new Color(255/255f, 230/255f, 128/255f, 255/255f);
            isSelected = true;
        } 
        else
        {             //작물이 심어져있다면 빨갛게 표시한다.
            farmSpriteRanderer.color =  new Color(255/255f ,85/255f, 85/255f, 255/255f);
            childSpriteRanderer.color = new Color(255/255f ,85/255f, 85/255f, 255/255f);
        }
    }

    public void HavastColorChange()
    {
        farmSpriteRanderer.color = originColor;
        childSpriteRanderer.color = originColor;
    }

    public void ReturnColor()
    {      //원래 색으로 변경
        farmSpriteRanderer.color = originColor;
        childSpriteRanderer.color = originColor;
        isSelected = false;
        plantMode = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {      //밭 터치 액션
        if (!afterPushFunc)
        {     //꾹 누르기 전 & 작물이 안심어져있다면
            parentFarmSelect.OnPointerDown(eventData);
            afterPushFunc = true;       //이후 취소해줘야함.
        }
        else if (afterPushFunc && isSelected && !isPlanted)
        {        //꾹 누른 후 원래작물은 심어져있지 않아있는 상태
            if (CropKind == null) return;
            int id = CropKind.GetComponent<CropGrowTime>().cropKind + 50;
            plantSound.Play();
            //Instanceate seedPrefab은 고쳐야함.
            Instantiate(CropKind, farmPosition + new Vector3(0.1f, 0.25f, 0.0f), Quaternion.identity);
            isPlanted = true;
            //심고나면 삭제
            IM.RemoveData(id, 1);
            //숫자 새로고침 해줘야함.
            cropSelectButton.ActiveCropBtn();
        }
    }
}