using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FarmUnlock : MonoBehaviour
{           
    // 빌드씬 9분할 해금
    public SpriteRenderer[] farmUnlock;

    private void Awake() {
        // 양식장 잠금상태 불러오기
        // 해금은 연구소에서 해금, 상태 저장
        EncryptedPlayerPrefs.SetInt($"FarmUnlock_{1}", 1);  // 혹시 모르니까 2번 농장 바닥 열어놓기
        for (int i=0; i<9; i++)
        {
            bool status = (EncryptedPlayerPrefs.GetInt($"FarmUnlock_{i}", 0) == 0);
            farmUnlock[i].gameObject.SetActive(status);
        }
    }
}