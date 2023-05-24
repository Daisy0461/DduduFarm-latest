using UnityEngine;

public class FishFarmUnlock : MonoBehaviour
{
    public GameObject[] fishArea;
    public GameObject[] fishfarmUnlock;
    
    private void Start() 
    {
        for (int i = 0; i < 3; i++)
        {
            var researchData = ResearchManager.Instance.GetDataExactly((int)DataTable.ResearchFish + i + 1);
            var isLock = researchData.IsResearched;
            fishfarmUnlock[i].SetActive(!isLock);
            fishArea[i].SetActive(isLock);
        }
    }
}
