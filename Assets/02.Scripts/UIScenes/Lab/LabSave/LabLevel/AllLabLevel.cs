using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All 부분은 아예 건들지 말기.
/// </summary>

[System.Serializable]
public class AllLabLevel
{
    public LabLevelSaveData[] labLevelSaveDatas;

    public AllLabLevel(LabLevelSaveData[] labLevelSaveDatas){
        this.labLevelSaveDatas = labLevelSaveDatas;
    }
}
