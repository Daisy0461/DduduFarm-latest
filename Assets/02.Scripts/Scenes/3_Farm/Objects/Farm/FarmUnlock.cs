using UnityEngine;

public class FarmUnlock : MonoBehaviour
{
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