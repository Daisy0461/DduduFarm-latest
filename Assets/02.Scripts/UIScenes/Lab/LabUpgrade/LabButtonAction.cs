using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabButtonAction : MonoBehaviour
{
    //LabButtonAction에 존재.
    private LabData labData;

    private void Start(){
        labData = GameObject.FindGameObjectWithTag("LabData").GetComponent<LabData>();
    }


    /// <summary>
    /// 1. upgrade 적용 함수 모음.
    /// </summary>
    public void FarmDecrease(){
        if(labData == null){
            Debug.Log("labData null");
        }
        labData.farmTimeDecrease = labData.farmTimeDecrease + labData.farmDecreaseRate;
    }
    public void FishDecrease(){
        if(labData == null){
            Debug.Log("labData null");
        }
        labData.fishTimeDecrease = labData.fishTimeDecrease + labData.fishDecreaseRate;
    }
    public void DuduOutgoingDecrease(){
        if(labData == null){
            Debug.Log("labData null");
        }
        labData.dduduOutgoingDecrease = labData.dduduOutgoingDecrease + labData.dduduOutgoingDecreaseRate;
    }
    public void DduduMakerDecrease(){
        if(labData == null){
            Debug.Log("labData null");
        }
        labData.dduduMakerDecrease = labData.dduduMakerDecrease + labData.dduduMakerDecreaseRate;
    }
}
