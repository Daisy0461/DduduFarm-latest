using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFarmUnlock : MonoBehaviour
{
    public GameObject[] fishArea;
    public GameObject[] fishfarmUnlock;
    
    private void Awake() {
        // 양식장 잠금상태 불러오기
        // 해금은 연구소에서 해금, 상태 저장
        EncryptedPlayerPrefs.SetInt($"FishFarmUnlock_{0}", 1);  // 혹시 모르니까 1번 양식장 열어놓기
        for (int i=0; i<4; i++)
        {
            bool status = (EncryptedPlayerPrefs.GetInt($"FishFarmUnlock_{i}", 0) == 0);
            fishfarmUnlock[i].SetActive(status);
            fishArea[i].SetActive(!status);
        }
    }


}
