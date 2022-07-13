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
    }

}
