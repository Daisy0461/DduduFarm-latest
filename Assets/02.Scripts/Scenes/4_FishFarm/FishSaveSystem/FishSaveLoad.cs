using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]       
public class FishDatas      //인스펙터에 표시하기 위해 이처럼 사용 
{     
    public GameObject[] savedFishLevel = new GameObject[10];    //fish의 종류 일단 10개로 잡았음. 뒤는 커지는 단계 0, 1, 2 /이건 Scriptable Object임.
}

public class FishSaveLoad : MonoBehaviour
{
    public FishDatas[] savedFishKind;                 //1번째 []사용은 fishKind, 두번쨰 .savedFishLevel은 fishLevel을 불러옴.

    public void SaveFish()
    {
        FishSaveManager.Save(GameObject.FindObjectsOfType<FishGrowTime>());        //Fish이거도 넣어줘
    }

    public void LoadFish(){
        AllFishsData save = FishSaveManager.Load();
        if (save == null) return;
        //태그 해줘야함! - 어려운거 아니니까 까먹지 말쟈~!
        GameObject[] fishs = GameObject.FindGameObjectsWithTag("Fish");

        for(int i=0; i<=save.fishSaveDatas.Length-1; i++){      //int i=save.farmSaveDatas.Length-1; i>=0; i--  //int i=0; i<=save.fishSaveDatas.Length-1; i++
            GameObject genFish = Instantiate(savedFishKind[save.fishSaveDatas[save.fishSaveDatas.Length-1-i].fishKind].
                                            savedFishLevel[save.fishSaveDatas[save.fishSaveDatas.Length-1-i].fishLevel],     //종류 선택!
                                            new Vector3(save.fishSaveDatas[save.fishSaveDatas.Length-1-i].x, save.fishSaveDatas[save.fishSaveDatas.Length-1-i].y, save.fishSaveDatas[save.fishSaveDatas.Length-1-i].z), 
                                            Quaternion.identity);

            FishGrowTime fishGrowTime = genFish.GetComponent<FishGrowTime>();
            fishGrowTime.remainGrowTime = save.fishSaveDatas[save.fishSaveDatas.Length-1-i].remainGrowTimeSave;
        }
        
        for(int i=0; i<fishs.Length; i++){
            Destroy(fishs[i]);
        }
    }
    
}