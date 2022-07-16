using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 건드릴 필요 없을 듯.
/// 저장되는 데이터
/// </summary>
public class LabData : LabSaveLoad
{
    //LabUpgrade
    [SerializeField]
    // 저장해야 되는 요소를 불러옴.
    // 연구 종류 탭 챕터를 넣어둠. Farm, Fish 등등
    private GameObject[] labUpgrades;  
    //농장
    //비료(재배속도) 연구 (%로 줄어듦)          Rate는 저장할 필요 없고 조정 가능.
    public int farmTimeDecrease = 0; [HideInInspector] public int farmDecreaseRate = 10;

    //낚시
    public int fishTimeDecrease = 0; [HideInInspector] public int fishDecreaseRate = 10;

    //뚜두
    public int dduduOutgoingDecrease = 0; [HideInInspector] public int dduduOutgoingDecreaseRate = 10;
    public int dduduMakerDecrease = 0; [HideInInspector] public int dduduMakerDecreaseRate = 10;
    
    private void Awake()
    {
        var obj = FindObjectsOfType<LabData>();
        if(obj.Length == 1){
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }

        LoadLabData();
    }

    public void OnApplicationFocus(bool value){
        if (value)
        {  
            LoadLabData();             //이까진 됐음.      - 잘 들고 온다.                  
        }
        else    //이탈 시 실행
        {
            SaveLabData();
        }
    }

    //게임 종료 시 실행되는 함수
    public void OnApplicationQuit()
    {
        SaveLabData();
    }
}
