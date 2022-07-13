using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//얘가 Data로 치면 된다.
[System.Serializable]
public class CropSaveData
{
   //시간
   public int remainGrowTimeSave;
   public int cropKind;
   public int cropLevel;

   public float[] position;
   public float x;
   public float y;
   public float z;

   public CropSaveData(CropGrowTime crop){
      //시간 저장
      remainGrowTimeSave = crop.remainGrowTime;
      cropKind = crop.cropKind;
      cropLevel = crop.cropLevel;

      x = crop.originPos.x;
      y = crop.originPos.y;
      z = crop.originPos.z;

      position = new float[3];
      position[0] = x;
      position[1] = y;
      position[2] = z;
   }
}
