using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishCount : MonoBehaviour
{
    [SerializeField] private Image EggImage;
    [SerializeField] private Text totalPriceText;   // 이름 변경 -> FishNameText 
    [SerializeField] private Text countText;
    [SerializeField] private GameObject[] fishKinds;
    [HideInInspector] public int fishEggId;
    
    private GameObject fishFarm_Be_Spawn;
    private Vector3 FarmPosition;
    private int _curFishCountToSpawn = 0;

    void OnEnable()
    {
        var fishInfo = ItemManager.Instance.GetInfo(fishEggId);
        EggImage.sprite = Resources.Load<Sprite>(fishInfo.imgPath);
        totalPriceText.text = fishInfo.name;
        countText.text = "1";
        _curFishCountToSpawn = 1;
    }

    public void IncreaseCount()
    {
        var researchBound = PlayerPrefs.GetFloat("수질 관리 시스템", 3);
        if (ItemManager.Instance.GetData(fishEggId).amount < _curFishCountToSpawn + 1 || researchBound <= _curFishCountToSpawn ) return;
        
        _curFishCountToSpawn++;
        countText.text = _curFishCountToSpawn.ToString();
    }

    public void DecreaseCount()
    {
        if(_curFishCountToSpawn <= 1) return;
            
        _curFishCountToSpawn--;
        countText.text = _curFishCountToSpawn.ToString();
    }

    //위에서 Increase or Decrease한 만큼 fish Spawn
    public void SpawnFish()
    {
        for(int i = 0; i < _curFishCountToSpawn; i++)
        {
            // 범위: x=-0.7~0.7 y=-0.35~0.35
            GameObject fish = Instantiate(fishKinds[fishEggId-(int)DataTable.FishEgg-1], 
                                        FarmPosition + new Vector3(Random.Range(-0.7f,0.7f), 
                                        Random.Range(-0.35f, 0.35f), 0.0f), Quaternion.identity);
            fish.transform.parent = fishFarm_Be_Spawn.transform;
            
        }
        ItemManager.Instance.RemoveData(fishEggId, _curFishCountToSpawn);
    }

    public void SetFishFarm(GameObject parent)
    {
        fishFarm_Be_Spawn = parent;
        FarmPosition = fishFarm_Be_Spawn.transform.position;
    }
}
