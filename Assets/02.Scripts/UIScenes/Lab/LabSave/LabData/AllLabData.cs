using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// LabSaveData �Ҵ�
/// </summary>
[System.Serializable]
public class AllLabData
{
    public LabSaveData labDatas;

    public AllLabData(LabSaveData labDatas){
        this.labDatas = labDatas;
    }
}