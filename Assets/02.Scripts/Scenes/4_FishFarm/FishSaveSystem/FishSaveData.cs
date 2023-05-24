using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishSaveData
{
   //시간
   public int remainGrowTimeSave;
   public int fishKind;
   public int fishLevel;

   public float[] position;
   public float x;
   public float y;
   public float z;

   public FishSaveData(FishGrowTime fish){
      //시간 저장
      remainGrowTimeSave = fish.remainGrowTime;
      fishKind = fish.fishKind;
      fishLevel = fish.fishLevel;

      x = fish.originPos.x;
      y = fish.originPos.y;
      z = fish.originPos.z;

      position = new float[3];
      position[0] = x;
      position[1] = y;
      position[2] = z;
   }
}
