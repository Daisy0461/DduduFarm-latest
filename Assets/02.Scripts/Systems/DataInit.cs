using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
[CustomEditor(typeof(DataInit))]

#endif // UNITY_EDITOR

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

#if UNITY_EDITOR

    

#endif // UNITY_EDITOR

}
