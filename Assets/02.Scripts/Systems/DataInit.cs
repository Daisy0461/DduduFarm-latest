using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInit : MonoBehaviour
{
    ItemManager IM;
    void Start()
    {
        IM = ItemManager.Instance;
        if (!IM.IsDataExist((int)DataTable.Love)) 
            IM.AddData((int)DataTable.Love, 5);
        else
        {
            if (IM.GetData((int)DataTable.Love).amount < 5)
                IM.AddData((int)DataTable.Love, 5);
        }
        if (!IM.IsDataExist((int)DataTable.Money)) 
            IM.AddData((int)DataTable.Money, 100000);
        else
        {
            if (IM.GetData((int)DataTable.Money).amount < 100000)
                IM.AddData((int)DataTable.Money, 100000);
        }
        
    }

}
