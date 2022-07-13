using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellCount : MonoBehaviour
{
    BuildingManager BM;
    public GameObject SellCountUI;
    public SellSlot slot;
    
    [SerializeField] private SellItemSlotCreate SISC;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text totalPriceText;
    [SerializeField] private Text countText;
    
    private void Start() 
    {
        BM = BuildingManager.Instance;    
    }

    public void OpenSellCountUI()   // 판매하기 - 나가기 중 판매하기를 클릭하면 SellCountUI 오픈 
    {
        slot = SISC.selectedSlot;
        if (slot == null) // 선택된 아이템이 없다면 아예 오픈하지 않음
            return;
        
        slot.GetComponent<Image>().color = Color.white;
        
        if (slot.iData.info.name != "")   // 잡화
        {
            itemNameText.text = slot.iData.info.name;
            if (slot.iData.info.sellCost == 0)
            {
                MoneyText.ChangeMoneyText(totalPriceText, slot.iData.info.buyCost/5);
                EncryptedPlayerPrefs.SetInt("ItemPrice", slot.iData.info.buyCost/5);
            }
            else
            {
                MoneyText.ChangeMoneyText(totalPriceText, slot.iData.info.sellCost);
                EncryptedPlayerPrefs.SetInt("ItemPrice", slot.iData.info.sellCost);
            } 
        } else {                    // 건설
            // itemNameText.text = slot.bCode.info.name;
            itemNameText.text = BM.GetInfo(slot.bCode).name;
            MoneyText.ChangeMoneyText(totalPriceText, BM.GetInfo(slot.bCode).sellCost);
            EncryptedPlayerPrefs.SetInt("ItemPrice", BM.GetInfo(slot.bCode).sellCost);
        }
        countText.text = "1";
        EncryptedPlayerPrefs.SetInt("BuyCount", 1);
       
        SellCountUI.SetActive(true);
    }

    public void SellCountDecrease()
    {
        int count = EncryptedPlayerPrefs.GetInt("BuyCount");
        if (count > 1)
        {
            EncryptedPlayerPrefs.SetInt("BuyCount", EncryptedPlayerPrefs.GetInt("BuyCount") - 1);
            countText.text = EncryptedPlayerPrefs.GetInt("BuyCount").ToString();
            MoneyText.ChangeMoneyText(totalPriceText, EncryptedPlayerPrefs.GetInt("BuyCount") * EncryptedPlayerPrefs.GetInt("ItemPrice"));
        }
        else if (count == 1)
        {
            int buyCount = 1;
            if (slot.bCode == 0) buyCount = slot.iData.amount;
            else buyCount = BM.GetBuildingAmount(slot.bCode) - BM.GetBuildedAmount(slot.bCode);
            EncryptedPlayerPrefs.SetInt("BuyCount", buyCount);
            countText.text = EncryptedPlayerPrefs.GetInt("BuyCount").ToString();
            MoneyText.ChangeMoneyText(totalPriceText, EncryptedPlayerPrefs.GetInt("BuyCount") * EncryptedPlayerPrefs.GetInt("ItemPrice"));    
        }
    }

    public void SellCountIncrease()
    {// 현재 가진 아이템의 개수까지만 팔 수 있다. 
        int count = EncryptedPlayerPrefs.GetInt("BuyCount");
        if ((slot.iData.info.name != "" && count < slot.iData.amount) || (slot.iData.info.name == "" && count < BM.GetBuildingAmount(slot.bCode)))
        {
            EncryptedPlayerPrefs.SetInt("BuyCount", EncryptedPlayerPrefs.GetInt("BuyCount") + 1);
            countText.text = EncryptedPlayerPrefs.GetInt("BuyCount").ToString();
            MoneyText.ChangeMoneyText(totalPriceText, EncryptedPlayerPrefs.GetInt("BuyCount") * EncryptedPlayerPrefs.GetInt("ItemPrice"));
        }
        else if ((slot.iData.info.name != "" && count == slot.iData.amount) || (slot.iData.info.name == "" && count == BM.GetBuildingAmount(slot.bCode)))
        {
            EncryptedPlayerPrefs.SetInt("BuyCount", 1);
            countText.text = EncryptedPlayerPrefs.GetInt("BuyCount").ToString();
            MoneyText.ChangeMoneyText(totalPriceText, EncryptedPlayerPrefs.GetInt("BuyCount") * EncryptedPlayerPrefs.GetInt("ItemPrice"));    
        }
    }

    // 판매하기 버튼 클릭 시 오픈되는 SellCountUI의 판매하기 버튼 정의
    public Text moneyText;
    public TalkTextScriptForSell talkTextScript;

    public void OnClickYes(GameObject sellUI){  // 판매하기 버튼
        ItemManager.Instance.AddData((int)DataTable.Money, (EncryptedPlayerPrefs.GetInt("ItemPrice") * EncryptedPlayerPrefs.GetInt("BuyCount")));
        MoneyText.ChangeMoneyText(moneyText, ItemManager.Instance.GetData((int)DataTable.Money).amount);
        talkTextScript.talkText.text = "";
        talkTextScript.BuyText();

        if (slot.iData.info.name != "") {   // 잡화 줄이기
            ItemManager.Instance.RemoveData(slot.iData.info.code, EncryptedPlayerPrefs.GetInt("BuyCount"));
            Text amtText = slot.transform.GetChild(0).GetComponentInChildren<Text>();
            amtText.text = slot.iData.amount.ToString();
            if (ItemManager.Instance.IsDataExist(slot.iData.id) == false)
            {
                SISC.PopSlot(slot.gameObject);
                Destroy(slot.gameObject); // 여기서 slots 에서 골라서 destroy할 수 있도록 하기
            }
                
        } else {    // 건물 줄이기
            BM.RemoveSomeData(slot.bCode, EncryptedPlayerPrefs.GetInt("BuyCount"));
            Text amtText = slot.transform.GetChild(0).GetComponentInChildren<Text>();
            amtText.text = BM.GetBuildingAmount(slot.bCode).ToString();
            if (BM.GetBuildingAmount(slot.bCode) <= 0)
            {
                SISC.PopSlot(slot.gameObject);
                Destroy(slot.gameObject);
            }
        }
        sellUI.SetActive(false);
        SISC.selectedSlot = null;
    }

    public void OnClickNo()
    {
        if (SISC.selectedSlot != null)
        {
            SISC.selectedSlot.GetComponent<Image>().color = Color.white;
            SISC.selectedSlot = null;
        }
        SellCountUI.SetActive(false);
    }
}
