using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTest : MonoBehaviour
{
    public int money;
    public Text moneyText;

    public void UpdateMoney()
    {
        ItemManager IM = ItemManager.Instance;
        IM.RemoveData((int)DataTable.Money, IM.GetData((int)DataTable.Money).amount);
        IM.AddData((int)DataTable.Money, money);
        // EncryptedPlayerPrefs.SetInt("Money", money);
        moneyText.text = string.Format("{0:#,##0}", IM.GetData((int)DataTable.Money).amount);
        // moneyText.text = string.Format("{0:#,##0}", EncryptedPlayerPrefs.GetInt("Money"));
    }
}
