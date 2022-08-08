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
    private SpriteRenderer farmSpriteRanderer;

    [SerializeField] [Tooltip ("자기 부모의 FarmSelect를 넣으면 됌.")]
    private FarmSelect parentFarmSelect;
    ItemManager IM;

    [Header ("선택된 씨앗")]
    public GameObject CropKind;

    [SerializeField]
    private GameObject warningPopup;
    [HideInInspector]
    public CropSelectButton cropSelectButton;
    [SerializeField]
    private AudioSource plantSound;
    private SpriteRenderer SR;

    void Start()
    {
        IM = ItemManager.Instance;
        farmPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        farmSpriteRanderer = gameObject.GetComponent<SpriteRenderer>();
        originColor = farmSpriteRanderer.color;
    }

    void Update()
    {
        if(isSelected){
            afterPushFunc = true;
        }
    }
 
    void OnCollisionEnter2D(Collision2D coll)   //작물이 들어올 때 작동 - 바꿔야하나?
    {   // 밭의 네모난 것과 작물이 충돌
        if(coll.gameObject.tag == "Crop"){
            farmSpriteRanderer.color =  originColor;
            isPlanted = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll){   //작물이 채집될 때 작동
        if(coll.gameObject.tag == "Crop"){
            isPlanted = false;
        }
    }

    public void ChangeFarmColor(){      //여기에 색 변화하게
        if(!isPlanted){     //작물이 심어져 있지 않은 상태라면 아래를 실행한다.
            farmSpriteRanderer.color =  new Color(255/255f, 230/255f, 128/255f, 255/255f);
            isSelected = true;
        }
    }

    public void ReturnColor(){      //원래 색으로 변경
        farmSpriteRanderer.color = originColor;
        isSelected = false;
    }

    public void OnPointerDown(PointerEventData eventData){      //밭 터치 액션
        if(!afterPushFunc){     //꾹 누르기 전
            parentFarmSelect.OnPointerDown(eventData);
            afterPushFunc = true;       //이후 취소해줘야함.
        }else if(afterPushFunc && isSelected && !isPlanted){        //꾹 누른 후 원래작물은 심어져있지 않아있는 상태
            if (CropKind == null) return;
            int id = CropKind.GetComponent<CropGrowTime>().cropKind + 50;
            //Audio
            plantSound.Play();
            //Instanceate seedPrefab은 고쳐야함.
            Instantiate(CropKind, farmPosition + new Vector3(0.1f, 0.25f, 0.0f), Quaternion.identity);
            isPlanted = true;
            //심고나면 삭제
            IM.RemoveData(id, 1);
            //숫자 새로고침 해줘야함.
            cropSelectButton.ActiveCropBtn();
            
        }else if(afterPushFunc && isSelected && isPlanted || afterPushFunc && isPlanted){     //꾹 누른 작업을 한 후 원래는 작물이 없었지만 심어서 생긴 경우 & 꾹 누른 후 원래 작물이 있는 경우
            //팝업 뜨게 하기
            warningPopup.SetActive(true);
        }
    }
}