using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllFishsData
{
    public FishSaveData[] fishSaveDatas;

    public AllFishsData(FishSaveData[] fishSaveDatas){
        this.fishSaveDatas = fishSaveDatas;
    }
}