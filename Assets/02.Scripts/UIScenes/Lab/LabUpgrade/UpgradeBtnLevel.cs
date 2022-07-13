using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeBtnLevel : MonoBehaviour
{
    //NOTE : 버튼에 들어가는 스크립트 텍스트로 표시해야할 것들을 모음.

    // 현재 레벨
    private UpgradeLevel thisLevel;
    // Don't Destroy 돼있는 오브젝트, 업그레이드 저장 데이터를 관리하는 오브젝트 -> 저장부랑 연결돼서 Sava&Load 가능
    private LabData labData;

    [SerializeField]
    // left : level, right : 레벨 변화 추이 텍스트, upgradeContent : 연구 효과 text, content text의 경우 따로 빼 둠.
    private Text levelText_left, levelText_right,upgradeContent;
    [SerializeField]
    // content text
    private string content;
    [SerializeField]
    // 효과 타입 -> type에 따라 reset_text 달라짐. 정수형이 아닌 연구에 대해 미리 대비하려고 만든 변수
    private string whatType;


    void Start()
    {  
        thisLevel = gameObject.GetComponent<UpgradeLevel>();    
        labData = labData = GameObject.FindGameObjectWithTag("LabData").GetComponent<LabData>();

        //Reset_Texts();    
    }

    /// <summary>
    /// 1. 업그레이드 UI Text 띄워주기
    /// 2. 연구 수치 가져와서 Text로 띄워주기
    /// </summary>
    public void Reset_Texts(){
        if(whatType == "int"){
            if(thisLevel.beforeUpgradeLevel == null || thisLevel.beforeUpgradeLevel_int == thisLevel.beforeGoolLevel_int){      //첫번째이거나 이전 것이 업그레이드가 완료되었거나
                string levelString = thisLevel.upgradeLevel.ToString();
                int nextLevel = thisLevel.upgradeLevel+1;

                levelText_left.text = (levelString + " LEVEL");
                levelText_right.text = ("레벨"+levelString + " -> " + "레벨" + (thisLevel.upgradeLevel+1).ToString());

                // upgrade : 현재 업그레이드에 따른 값을 저장하는 변수
                int upgrade = SelectWhichUpgrade_int();
                // upgadeRate : 업그레이드 증가 폭
                int upgradeRate = SelectWhichUpgradeRate_int();
                upgradeContent.text = (upgrade.ToString() + "%  ->  " + (upgrade+upgradeRate).ToString() + "%");
            }
        }
    }
    
    /// <summary>
    /// 1. content 에 따라 현재 업그레이드에 따른 값을 반환하는 함수.
    /// </summary>
    /// <returns></returns>
    private int SelectWhichUpgrade_int(){       //int값을 반환하는 연구에 대한 선택.
        if(content == "farmTimeDecrease"){        //변수명 동일하게 적는게 안헷갈리고 좋을듯.
            return labData.farmTimeDecrease;
        }else if(content == "fishTimeDecrease"){
            return labData.fishTimeDecrease;
        }else if (content == "dduduOutgoingDecrease"){
            return labData.dduduOutgoingDecrease;
        }else if (content == "dduduMakerDecrease"){
            return labData.dduduMakerDecrease;
        }
        return 0;

    }
    
    /// <summary>
    /// 1. content 에 따라 현재 업그레이드 증가 폭에 따른 반환하는 함수.
    /// </summary>
    /// <returns></returns>
    private int SelectWhichUpgradeRate_int(){       //글자 띄어쓰기까지 제대로 작성해야함.
        if(content == "farmTimeDecrease"){   
            return labData. farmDecreaseRate;
        }else if(content == "fishTimeDecrease"){
            return labData.fishDecreaseRate;
        }else if (content == "dduduOutgoingDecrease"){
            return labData.dduduOutgoingDecreaseRate;
        }else if (content == "dduduMakerDecrease"){
            return labData.dduduMakerDecreaseRate;
        }
        return 0;
    }

}
