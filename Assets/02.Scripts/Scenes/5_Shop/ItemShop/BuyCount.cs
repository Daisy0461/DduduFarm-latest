using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyCount : MonoBehaviour
{// 아이템 개수 카운팅
    [SerializeField] private Text countText;
    [SerializeField] private Text totalPriceText;

    public void BuyCountDecrease()
    {
        EncryptedPlayerPrefs.SetInt("BuyCount", EncryptedPlayerPrefs.GetInt("BuyCount") - 1);
        if (EncryptedPlayerPrefs.GetInt("BuyCount") < 1)
        {
            EncryptedPlayerPrefs.SetInt("BuyCount", 1);
            countText.text = EncryptedPlayerPrefs.GetInt("BuyCount").ToString();
            totalPriceText.text = (1 * EncryptedPlayerPrefs.GetInt("ItemPrice")).ToString();
        }
        else
        {
            countText.text = EncryptedPlayerPrefs.GetInt("BuyCount").ToString();
            totalPriceText.text = (EncryptedPlayerPrefs.GetInt("BuyCount") * EncryptedPlayerPrefs.GetInt("ItemPrice")).ToString();
        }
    }

    public void BuyCountIncrease()
    {
        int buyCount = EncryptedPlayerPrefs.GetInt("BuyCount");
        if ((buyCount + 1) * EncryptedPlayerPrefs.GetInt("ItemPrice") <= ItemManager.Instance.GetData((int)DataTable.Money).amount)
        {
            EncryptedPlayerPrefs.SetInt("BuyCount", buyCount + 1);
            countText.text = EncryptedPlayerPrefs.GetInt("BuyCount").ToString();
            totalPriceText.text = (EncryptedPlayerPrefs.GetInt("BuyCount") * EncryptedPlayerPrefs.GetInt("ItemPrice")).ToString();
        }
    }
}
