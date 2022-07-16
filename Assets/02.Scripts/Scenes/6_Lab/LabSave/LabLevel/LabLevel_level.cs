using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LabLevel_level : LabLevelSaveLoad
{
    //게임 초기화(Awake), 중간 이탈, 중간 복귀 시 실행되는 함수
    [SerializeField]
    private GameObject[] labUpgrades; 

    public void OnApplicationFocus(bool value){
        if (value)
        {  
            ActiveAndLoad();                         
        }
        else    //이탈 시 실행
        {
            ActiveAndSave();
        }
    }

    //게임 종료 시 실행되는 함수
    public void OnApplicationQuit()
    {
        ActiveAndSave();   
    }

    public void ActiveAndSave(){
        for(int i=0; i<labUpgrades.Length; i++){
            labUpgrades[i].SetActive(true);
        }
        SaveLabLevel();
        for(int i=0; i<labUpgrades.Length; i++){
            labUpgrades[i].SetActive(false);
        }
    }

    public void ActiveAndLoad(){
        for(int i=0; i<labUpgrades.Length; i++){
            labUpgrades[i].SetActive(true);
        }                           
        LoadLabLevel();
        for(int i=0; i<labUpgrades.Length; i++){
            labUpgrades[i].SetActive(false);
        }
    }

}
