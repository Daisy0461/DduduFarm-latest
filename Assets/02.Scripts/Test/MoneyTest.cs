using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTest : MonoBehaviour
{
    public int money;

    public void UpdateMoney()
    {
        ItemManager IM = ItemManager.Instance;
        IM.RemoveData((int)DataTable.Money, IM.GetData((int)DataTable.Money).amount);
        IM.AddData((int)DataTable.Money, money);
    }
}
