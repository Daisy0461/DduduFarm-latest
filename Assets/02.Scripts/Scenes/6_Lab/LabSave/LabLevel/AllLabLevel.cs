using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All �κ��� �ƿ� �ǵ��� ����.
/// </summary>

[System.Serializable]
public class AllLabLevel
{
    public LabLevelSaveData[] labLevelSaveDatas;

    public AllLabLevel(LabLevelSaveData[] labLevelSaveDatas){
        this.labLevelSaveDatas = labLevelSaveDatas;
    }
}
