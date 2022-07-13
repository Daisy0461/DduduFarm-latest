using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]       
public class CropDatas      //인스펙터에 표시하기 위해 이처럼 사용 
{     
    public GameObject[] savedCropLevel = new GameObject[10];    //crop의 종류 일단 10개로 잡았음. 뒤는 커지는 단계 0, 1, 2 /이건 Scriptable Object임.
}

public class CropSaveLoad : MonoBehaviour
{
    public CropDatas[] savedCropKind;                 //1번째 []사용은 cropKind, 두번쨰 .savedCropLevel은 cropLevel을 불러옴.

    public void SaveCrop(){
        CropSaveManager.Save(GameObject.FindObjectsOfType<CropGrowTime>());        //Crop이거도 넣어줘
        //Debug.Log("CropSaveLoad에서 Save실행 끝");
    }

    public void LoadCrop(){
        AllCropsData save = CropSaveManager.Load();
        if (save == null) return;

        GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");

        for(int i=0; i<=save.cropSaveDatas.Length-1; i++){      //int i=save.farmSaveDatas.Length-1; i>=0; i--  //int i=0; i<=save.cropSaveDatas.Length-1; i++
            GameObject genCrop = Instantiate(savedCropKind[save.cropSaveDatas[save.cropSaveDatas.Length-1-i].cropKind - (int)DataTable.Crop - 1].
                                            savedCropLevel[save.cropSaveDatas[save.cropSaveDatas.Length-1-i].cropLevel],     //종류 선택!
                                            new Vector3(save.cropSaveDatas[save.cropSaveDatas.Length-1-i].x, save.cropSaveDatas[save.cropSaveDatas.Length-1-i].y, save.cropSaveDatas[save.cropSaveDatas.Length-1-i].z), 
                                            Quaternion.identity);

            CropGrowTime cropGrowTime = genCrop.GetComponent<CropGrowTime>();
            cropGrowTime.remainGrowTime = save.cropSaveDatas[save.cropSaveDatas.Length-1-i].remainGrowTimeSave;
        }
            //Debug.Log("CropSaveLoad의 Load 끝 즉 remainGrowTime이 Crop에 전해짐.");
        for(int i=0; i<crops.Length; i++){
            Destroy(crops[i]);
        }
    }
}