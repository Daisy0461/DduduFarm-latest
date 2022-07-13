using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCountUI : MonoBehaviour
{
    //버튼에 존재하는 스크립트 - 이름을 바꿔야하는데
    public GameObject fishFarm_Be_Spawn;        //터치로 인해 결정이 된다.
    private Vector3 FarmPosition;
    private GameObject fish;

    public void SetFishFarm(GameObject fishFarm){
        //어떤 FishFarm을 골랐는지 알게 하기 위해서 들어있는 것.
        fishFarm_Be_Spawn = fishFarm;
        FarmPosition = fishFarm.transform.position;
    }

    // 쓰이는 곳이 없는 것으로 보임
    // public GameObject GetFishFarm(){
    //     return fishFarm_Be_Spawn;
    // }

    public void OpenCount(GameObject ui){ // ui: 물고기 알 개수 선택 팝업. 물고기 종류 선택하기
        EncryptedPlayerPrefs.SetInt("FishCount", 1);
        ui.SetActive(true);
        FishCount fishCount = ui.GetComponent<FishCount>();
        fishCount.SetFishFarm(fishFarm_Be_Spawn);
    }
}
