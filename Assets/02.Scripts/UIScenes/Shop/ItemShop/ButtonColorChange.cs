using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChange : MonoBehaviour
{// 아이템 선택 후 구매하기 버튼 클릭 시 구매하기 버튼의 색깔 바꾸기
    [SerializeField]
    private Text buyText;
    [SerializeField]
    private Text exitText;

    private Color originColor;
    
    public ShopItemSlotCreate SIUI;

    void Start(){
        originColor = buyText.color;
    }

    public void ChangeBuyButtonColor(){
        if (SIUI.selectedShopItem == null) // 선택된 아이템이 없다면 아예 오픈하지 않음
            return;
        buyText.color = new Color(255, 243, 216);
        // Debug.Log("흰색으로 바꿈");
    }

    public void ChangeExitButtonColor(){
        exitText.color = new Color(255, 243, 216);
    }

    public void BackBuyButtonColor(){
        buyText.color = originColor;
        // Debug.Log("초록으로 바꿈");
    }
}
