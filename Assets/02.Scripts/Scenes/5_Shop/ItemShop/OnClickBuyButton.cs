using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickBuyButton : MonoBehaviour
{
    [SerializeField] private GameObject buyCountUI;
    [SerializeField] private Text buyCountText;
    [SerializeField] private Text totalBuyPrice;
    [SerializeField] private Text nameText;
    [SerializeField] private Text moneyText;
    [SerializeField] private TalkTextScript talkTextScript;
    [SerializeField] private GameObject refuseUI;
    [SerializeField] private ShopItemSlotCreate SIUI;

    public void FirstMoneyCheck(){
        if (SIUI.selectedShopItem == null) // 선택된 아이템이 없다면 아예 오픈하지 않음
            return;
        
        SIUI.selectedShopItem.GetComponent<Image>().color = Color.white;
        
        // 돈이 있는지 확인
        if (ItemManager.Instance.GetData((int)DataTable.Money).amount - EncryptedPlayerPrefs.GetInt("ItemPrice") < 0.0f)
        {
            refuseUI.SetActive(true);
            SIUI.selectedShopItem = null;   
        }
        else
        {
            buyCountUI.SetActive(true); // 구매 개수 설정하는 UI
            
            EncryptedPlayerPrefs.SetInt("BuyCount", 1);
            buyCountText.text = EncryptedPlayerPrefs.GetInt("BuyCount").ToString();
            nameText.text = SIUI.selectedShopItem.info.name;
            totalBuyPrice.text = (EncryptedPlayerPrefs.GetInt("BuyCount") * EncryptedPlayerPrefs.GetInt("ItemPrice")).ToString();
        }
    }

    public void LastMoneyChek() 
    {
        if (ItemManager.Instance.GetData((int)DataTable.Money).amount - (EncryptedPlayerPrefs.GetInt("ItemPrice") * EncryptedPlayerPrefs.GetInt("BuyCount")) < 0.0f)
        {
            buyCountUI.SetActive(false);
            refuseUI.SetActive(true);
            SIUI.selectedShopItem = null;
            SIUI.audioSource.Play();
        }
        else    // 산다
        {
            if (SIUI.OnClickShopItemBuy() == false)
            {
                SIUI.audioSource.PlayOneShot(SIUI.coinSound);
                buyCountUI.SetActive(false);
                return;
            }

            ItemManager.Instance.RemoveData((int)DataTable.Money, (EncryptedPlayerPrefs.GetInt("ItemPrice") * EncryptedPlayerPrefs.GetInt("BuyCount")));
            // EncryptedPlayerPrefs.SetInt("Money", EncryptedPlayerPrefs.GetInt("Money") - (EncryptedPlayerPrefs.GetInt("ItemPrice") * EncryptedPlayerPrefs.GetInt("BuyCount")));
            MoneyText.ChangeMoneyText(moneyText, ItemManager.Instance.GetData((int)DataTable.Money).amount);
            talkTextScript.BuyText();
            buyCountUI.SetActive(false);
            SIUI.audioSource.PlayOneShot(SIUI.coinSound);
        }
    }

    public void OnClickBuyNo()
    {            
        if (SIUI.selectedShopItem != null)
        {
            SIUI.selectedShopItem.GetComponent<Image>().color = Color.white;
            SIUI.selectedShopItem = null;
        }
        buyCountUI.SetActive(false);
    }
}
