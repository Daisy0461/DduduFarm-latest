using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSelectButton : MonoBehaviour           //Crop 심는거 버튼 이벤트.
{
    [SerializeField] private GameObject[] cropSlots;
    [SerializeField] private CropButtonColorChange[] changeSlotColor;
    [SerializeField] private TextObject[] seedSlots;

    [HideInInspector]
    public GameObject selectedCropSlot;
    ItemManager IM;
    [SerializeField]
    FarmFind farmFind;
    private bool didSetCropButton = false;
    private int count=0, checkCount;        //checkCount는 이전의 카운트틀 담음

    private void OnEnable() 
    {
        IM = ItemManager.Instance;
        ActiveCropBtn();
        if(!didSetCropButton){
            for(int i=0; i<farmFind.farmStateList.Length; i++){
                farmFind.farmStateList[i].cropSelectButton = this;
            }
            didSetCropButton = true;
        }
    }

    public void ActiveCropBtn()
    {
        checkCount = count;     //이전의 count를 담아옴
        count = 0;      //count 초기화
        for(int id=0; id<cropSlots.Length; id++)
        { 
            if (IM.GetData(id + (int)DataTable.Seed + 1) != null)
            {
                count++;        //몇개가 활성화 되는지 체크
                cropSlots[id].SetActive(true);
                var seedCount = IM.GetData(id + (int)DataTable.Seed + 1);
                seedSlots[id].contentText.text = seedCount.amount.ToString();
            }
            else {
                cropSlots[id].SetActive(false);
            }
        }

        if(checkCount != count){        //이전의 활성화된 Slot갯수와 동일하지 않다 -> 하나가 꺼졌다 -> 씨앗을 다 썼다는 의미
            ClickCropButton(null);
        }
    }

    public void ClickCropButton(GameObject CropKind)        //cropKind정함.
    {
        for(int i=0; i<farmFind.farmStateList.Length; i++){
            farmFind.farmStateList[i].CropKind = CropKind;
        }
    }

    public void CheckSelectedButton(GameObject button){
        for(int i=0; i<changeSlotColor.Length; i++){
            changeSlotColor[i].checkButton = button;
        }

        for(int i=0; i<changeSlotColor.Length; i++){
            changeSlotColor[i].ChangeButtonColor();
        }
    }

}