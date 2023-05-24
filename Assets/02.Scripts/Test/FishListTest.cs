using UnityEngine;

public class FishListTest : MonoBehaviour
{
    [HideInInspector] public int val;

    public void SetFish()
    {
        for(int i=1; i<=5; i++)
        {
            ItemManager.Instance.AddData((int)DataTable.Fish + i, 10);
        }
    }

    public void SetCrop()
    {
        for (int i=1; i<=5; i++)
        {
            ItemManager.Instance.AddData((int)DataTable.Crop + i, 100);
        }
    }

    public void SetMoney()
    {
        ItemManager.Instance.AddData((int)DataTable.Money, val);
    }

    public void SetBuildings()
    {
        for (int i=1; i<=8; i++)
        {
            BuildingManager.Instance.AddData(i);
        }
        for (int i=51; i<=60; i++)
        {
            BuildingManager.Instance.AddData(i);
        }
    }
}
