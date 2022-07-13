using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeLevel : MonoBehaviour
{
    //List에서 버튼 클릭시 창을 띄우는 스크립트임.
    //각각 버튼에 존재 -> Inspector 가독성을 위한 코드
    [Header ("Levels")]

    // 기존 기획의 산물 : 아래 메서드 확인해보고, 연결만 잘 해주기.
    [Tooltip ("업그레이드 이전 레벨을 나타냄.")]
    public int upgradeLevel = 0;
    [Tooltip ("업그레이드가 완료 되었을 떄의 Level")]
    public int goolLevel = 0;

    [Space (10)]
    // 이전 연구항목 버튼 오브젝트
    public UpgradeLevel beforeUpgradeLevel;

    private UpgradeBtnLevel upgradeBtnLevel;
    private int upgradeCheck;

    // 연구가 가능하다면 true
    private bool buttonAction = false;

    // 이전 연구 항목에 대한 레벨 값을 정수형으로 저장하는 변수
    [HideInInspector]
    public int beforeUpgradeLevel_int;
    [HideInInspector]
    public int beforeGoolLevel_int;
    
    // 가지로 나뉘어지는 부분
    [Tooltip ("이후 버튼이 1개일 경우 nextUp1부터 채워야함.")]
    public UpgradeLevel nextUp1, nextUp2;

    [SerializeField]
    private GameObject maxUpgradeUI;

    // 연구 완료 했을 때 나오는 이미지
    [SerializeField]
    private Sprite upgradeCompleteImage;

    void Start(){
        // 버튼 스크립트 가져오는 부분
        upgradeBtnLevel = gameObject.GetComponent<UpgradeBtnLevel>();

        if(beforeUpgradeLevel == null){      //첫번째라면
            // dim 처리가 돼있지 않으면 true
            buttonAction = true;
        }else{
            buttonLevelCheck_Dim();
        }

        // 최종 연구가 아닌 연구 항목 하나가 됐을 때 실행하도록 수정 
        if(upgradeLevel == goolLevel){
            Image thisImage = gameObject.GetComponent<Image>();
            thisImage.sprite = upgradeCompleteImage;
        }
    }

    /// <summary>
    /// 1. Onclick() -> ButtonActive method -> 인자로 업그레이드 UI 띄워주는 부분
    /// -> 결론은 연구가 완료되지 않았으면 클릭 시 UI 나오고, 완료됐으면 안 나온다. 
    /// -> 완료 됐으면 완료 정보 띄워줘도 될 듯
    /// 구조 잘 생각해보기.
    /// </summary>
    /// <param name="upgradeUI"></param>
    // goolLevel 부분 -> 기존 기획 산물 : 전체 수정할 것.
    public void buttonActive(GameObject upgradeUI){     //스크롤 뷰 안에 있는 버튼을 클릭했을 때 실행되는 버튼액션.
        if(buttonAction && goolLevel != upgradeLevel){       //업그레이드가 아직 진행되지 않음.
            upgradeUI.SetActive(true);
        }
        
        // 연구 완료 항목에 대한 정보를 표시하는 코드 : 삭제해도 되고, 그대로 둬도 됨. goolLevel 수정
        else if(buttonAction && goolLevel == upgradeLevel){     //업그레이드가 진행 완료됌.  -> 기능 없음으로 될 수도 있음.
            // 연구 완료 항목에 대한 정보를 띄워주는 UI -> 인자를 하나만 설정하려고 전역으로 설정함.
            if(maxUpgradeUI != null){
                maxUpgradeUI.SetActive(true);
            }
        }
    }

    //딤드 처리 함수
    /// <summary>
    /// 1. 이전 연구 항목 레벨 불러오기
    /// 2. 업그레이드 UI에서 Level Text 표기해주기
    /// 3. 연구 진행 여부에 따라 Dim 처리해주기
    /// </summary>
    public void buttonLevelCheck_Dim(){
        // 기존 기획 산물 : beforUpgradeLevel - 이전 연구 항목, before~_int : 정수형으로 읽어온 이전 연구 항목 레벨 -> 삭제 가능
        beforeUpgradeLevel_int = beforeUpgradeLevel.upgradeLevel;                         //이전 버튼의 level을 들고옴.
        beforeGoolLevel_int = beforeUpgradeLevel.goolLevel;

        // 기존 기획 산물로 추정 : 확인해보고 없앨 것.
        if(upgradeBtnLevel != null){
            upgradeBtnLevel.Reset_Texts(); // UI에서 레벨 어떻게 바뀌는지 표시해주는 부분.
        }

        // 기존 기획에서는 여러 번 업그레이드가 가능해서 beforeGoolLevel 값에 자유롭게 값 할당이 가능했지만
        // 한 연구항목은 한 번만 업그레이드 되게 수정함에 따라 GoolLevel = 1, 연구항목 레벨을 0으로 설정해서 연구를 진행했는지만 확인함.
        // boolean 으로 설정해도 가능.
        if(beforeUpgradeLevel_int != beforeGoolLevel_int){      //이전 버튼이 업그레이드 되지 않았으면 실행.
                gameObject.GetComponent<Image>().color = Color.gray;     
                buttonAction = false;
        }else{
                gameObject.GetComponent<Image>().color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
                buttonAction = true;
        }
    }

    /// <summary>
    /// 1. 연구를 하면 레벨 업
    /// 2. 레벨업하면 이미지 CompleteImage로 변경해주기. -> Image 바꾸는 건 유지.
    /// </summary>
    public void LevelUp(){
        this.upgradeLevel++;
        Image thisImage = gameObject.GetComponent<Image>();
        thisImage.sprite = upgradeCompleteImage;
    }
}
