using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LabSaveData
{
    // 여기도 연구 항목 수치 추가.
    public int farmTimeDecrease = 0;

    // 연구 항목 조정 수치에 대한 메서드 다른 스크립트에서 처리. 여기는 저장만
    // LabSaveData 메서드 내부에 연구 항목에 대한 수치 할당.

    // Start is called before the first frame update
    public LabSaveData(LabData data){
        farmTimeDecrease = data.farmTimeDecrease;
    }
}
