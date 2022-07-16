using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 연구 완료 여부를 int 가 아닌 bool 로 판단할 경우, 이 부분에서 int 형을 bool 로 바꿔 저장하게 하면 됨.
/// </summary>
[System.Serializable]
public class LabLevelSaveData
{
    public int level;

    public LabLevelSaveData(UpgradeLevel up)
    {
        level = up.upgradeLevel;
    }
}
