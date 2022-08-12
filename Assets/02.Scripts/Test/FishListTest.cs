using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishListTest : MonoBehaviour
{
    BuildingManager BM;
    ItemManager IM;
    [HideInInspector] public int val;
    private void Start() 
    {
        IM = ItemManager.Instance;
        BM = BuildingManager.Instance;
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

    public void SetBuildings()
    {
        for (int i=1; i<=8; i++)
        {
            BM.AddData(i);
        }
        for (int i=51; i<=60; i++)
        {
            BM.AddData(i);
        }
    }
}
