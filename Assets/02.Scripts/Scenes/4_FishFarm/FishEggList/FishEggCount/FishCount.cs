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
    ItemManager IM;

    void OnEnable(){
        IM = ItemManager.Instance;
        // 이미지 
        EggImage.sprite = Resources.Load<Sprite>(IM.GetInfo(fishEggId).imgPath);
        totalPriceText.text = IM.GetInfo(fishEggId).name;
        countText.text = "1";
        EncryptedPlayerPrefs.SetInt("FishCount", 1);
    }

    //Fish Spawn하는 개수 Increase
    public void IncreaseCount(){
        if(EncryptedPlayerPrefs.GetInt("FishCount") <= 2 && IM.GetData(fishEggId).amount > 0){
            EncryptedPlayerPrefs.SetInt("FishCount", EncryptedPlayerPrefs.GetInt("FishCount") + 1);
            // totalPriceText.text = (EncryptedPlayerPrefs.GetInt("FishCount") * fishPrice).ToString();
            countText.text = (EncryptedPlayerPrefs.GetInt("FishCount")).ToString();
        }
        
    }

    //Fish Spawn하는 개수 Decrease
    public void DecreaseCount(){
        if(EncryptedPlayerPrefs.GetInt("FishCount") > 1){
            EncryptedPlayerPrefs.SetInt("FishCount", EncryptedPlayerPrefs.GetInt("FishCount") - 1);
            // totalPriceText.text = (EncryptedPlayerPrefs.GetInt("FishCount") * fishPrice).ToString();
            countText.text = (EncryptedPlayerPrefs.GetInt("FishCount")).ToString();
        }
    }

    //위에서 Increase or Decrease한 만큼 fish Spawn
    public void SpawnFish(){
        int count = EncryptedPlayerPrefs.GetInt("FishCount");
        for(int i=0; i<count; i++)
        {
            //x=-0.7~0.7 y=-0.35~0.35 범위
            GameObject fish = Instantiate(fishKinds[fishEggId-(int)DataTable.FishEgg-1], 
                                        FarmPosition + new Vector3(Random.Range(-0.7f,0.7f), 
                                        Random.Range(-0.35f, 0.35f), 0.0f), Quaternion.identity);
            fish.transform.parent = fishFarm_Be_Spawn.transform;
            
        }
        IM.RemoveData(fishEggId, count);
    }

    public void SetFishFarm(GameObject parent){
        fishFarm_Be_Spawn = parent;
        FarmPosition = fishFarm_Be_Spawn.transform.position;
    }
}
