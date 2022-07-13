using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���� �Ϸ� ���θ� int �� �ƴ� bool �� �Ǵ��� ���, �� �κп��� int ���� bool �� �ٲ� �����ϰ� �ϸ� ��.
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
