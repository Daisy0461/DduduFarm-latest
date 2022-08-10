using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishListTest : MonoBehaviour
{
    ItemManager IM;
    [HideInInspector] public int val;
    private void Start() 
    {
        IM = ItemManager.Instance;
    }

    public void SetFish()
    {
        for(int i=1; i<=5; i++)
        {
            IM.AddData((int)DataTable.Fish + i, 10);
        }
    }

    public void SetCrop()
    {
        for (int i=1; i<=5; i++)
        {
            IM.AddData((int)DataTable.Crop + i, 100);
        }
    }

    public void SetMoney()
    {
        IM.AddData((int)DataTable.Money, val);
    }
}
