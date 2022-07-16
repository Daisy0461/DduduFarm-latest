using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// LabLevel Tag 의 오브젝트로 불러와서 저장, 불러오기 하는 스크립트
/// </summary>
public class LabLevelSaveLoad : MonoBehaviour
{
    public void SaveLabLevel(){
        GameObject[] UpgradeLevel = GameObject.FindGameObjectsWithTag("LabLevel");
        UpgradeLevel[] upgradeLevels = GameObject.FindObjectsOfType<UpgradeLevel>();
        for(int i=0; i<UpgradeLevel.Length; i++){
            //Debug.Log("Save될 때 오브젝트 순서: " + UpgradeLevel[i].name);
            upgradeLevels[i] = UpgradeLevel[i].GetComponent<UpgradeLevel>();
        }
        LabLevelSaveManager.Save(upgradeLevels);
    }

    /// <summary>
    /// bool로 바꾸려면 여기를 수정해야됨.
    /// 순서 고려(순서 복잡함) -> 순서가 다르면 다른 곳에 로드됨 ??
    /// 불러올 때 grid 위치에 따라 할당해야하기에 순서 잘 고려해야 됨
    /// 데이터 이상하게 할당되면 바로 재준이 호출
    /// 웬만하면 손 대지 말 것. -> 안 건드는 게 Best
    /// </summary>
    public void LoadLabLevel(){
        AllLabLevel save = LabLevelSaveManager.Load();
        //태그 해줘야함! - 어려운거 아니니까 까먹지 말쟈~!
        GameObject[] UpgradeLevel = GameObject.FindGameObjectsWithTag("LabLevel");      //type은 활성화 되어있는 것만 가능함.. 그래서 Tag로 활용

        //Debug.Log("저장된 연구소 버튼 길이: " + save.labLevelSaveDatas.Length); //- 전체 찾음.
        //Debug.Log("태그로 찾아진 오브젝트의 갯수: " + UpgradeLevel.Length);
        for(int i=save.labLevelSaveDatas.Length-1; i>=0; i--){                          //int i=0; i<=save.labLevelSaveDatas.Length-1; i++      -> 뒤집히는 상태.
            //Debug.Log("로드되는 연구소 버튼의 이름: " + UpgradeLevel[i].name);
            //Debug.Log("위 오브젝트에 할당되는 값: " + save.labLevelSaveDatas[save.labLevelSaveDatas.Length-1-i].level);       //이게 지금 뒤집히는 상태.
            UpgradeLevel level = UpgradeLevel[i].GetComponent<UpgradeLevel>();
            level.upgradeLevel = save.labLevelSaveDatas[i].level;
        }
    }
}