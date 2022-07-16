using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 직렬화
/// 쓴 이유 : inspector 창에서 2차원 배열을 쓰기 위해서 -> 기존 기획, 삭제
/// </summary>
[System.Serializable]
public  class IngredientCountArray
{     
    public int[] CountArray;
}
[System.Serializable]
public  class IngredientIDArray
{     
    public int[] IdArray;
}

public class SatisfyCheck : MonoBehaviour
{
    //연구를 하는 스크립트 Ingredient 이미지들의 부모에 존재.
    public bool canUpgrade = true;

    //??
    [SerializeField] private LabButtonAction labButtonAction;

    // 연구를 할 때 필요한 재료 종류의 개수
    // 한 개를 계속 업그레이드 했기 때문에 배열로 했지만, 기획이 변경됨에 따라 배열 없이 가능 -> 필요한 종류의 개수 countArray, IdArray 의 인덱스 값을 받아오는 방식으로 삭제 가능
    [SerializeField] [Tooltip ("복사되는 재료의 갯수")] private int[] ingredientsCounts;
    // default 판 -> 프리팹 사용하고, 재료 슬롯의 기본형 아무것도 없는!
    [SerializeField] private GameObject ingredientGameobject;

    [SerializeField] private GridLayoutGroup group;     //해당 스크롤뷰의 그리드레이아웃 넣으면 됨. (content에 있음.)
    // 해당 업그레이드 버튼을 넣으면 됌, ingredientGameobject 안에 ingredientcheck 스크립트가 들어가있음. -> 재료 아이디랑 개수 다 들어가있음.
    private IngredientCheck[] ingredients;

    //??
    [Tooltip ("해당 업그레이드를 하는 버튼을 순차적으로 넣으면 됩니다.")]
    public GameObject[] upgradeBtns;

    //?? 기존 기획, 계속 업그레이드를 할 때 버튼의 레벨을 동기화하려고 만든 것
    [SerializeField]
    private UpgradeBtnLevel upgradeBtnLevel;    //얘는 뭐냐;;
    private UpgradeLevel BtnUpgradeLevel;       //upgradeBtn의 upgradeLevel을 들고온다.

    // 버튼 레벨
    private int level;

    /// <summary>
    /// 기존 기획 -> 현재 기획으로 적용을 하면서 이차원 배열을 사용할 필요가 없어졌으니, 일차원 배열로 설정을 해서 항목별로 연구 재료 넣어놓기.
    /// </summary>
    [SerializeField] [Tooltip ("필요한 재료의 갯수")]
    private IngredientCountArray[] countArray;
    [SerializeField] [Tooltip ("필요한 재료의 id 값")]
    private IngredientIDArray[] IdArray;

    // delegate를 쓴 이유 : 추후 관리를 하려고 사용했었음.
    delegate void ResetIngredientsLevelBtn();
    ResetIngredientsLevelBtn resetIngredientsLevelBtn;

    void Start(){
        //레벨을 받아와서 배열 몇번째인지 확인하고 함.
        resetIngredientsLevelBtn += SetLevelAndBtn;
        resetIngredientsLevelBtn += ResetCanUpgrade;

        resetIngredientsLevelBtn();
        ReadjustGrid(ingredientsCounts[level]);
    }

    /// <summary>
    /// 옛날 기획 -> 삭제
    /// 1. 주변 연구 항목의 레벨이 이번 연구 항목의 완료 레벨과 같다면 다음 탐색으로 넘어간다.
    /// 2. 아니라면 현재 레벨을 해당 레벨로 설정한다. (레벨이 아니라 연구 완료 여부를 체크해야되지 않나? ㅠㅠ)
    /// </summary>
    private void SetLevelAndBtn(){      //버튼과 level을 정함.
        for(int i = 0; i<upgradeBtns.Length; i++){
            BtnUpgradeLevel = upgradeBtns[i].GetComponent<UpgradeLevel>();      //순사척으로 컴포넌트를 들고온다.
            if(BtnUpgradeLevel.upgradeLevel == BtnUpgradeLevel.goolLevel){      //업그레이드가 완료되었다면
                continue;
            }else{
                level = BtnUpgradeLevel.upgradeLevel;
                break;
            }
        }
    }

    /// <summary>
    /// 옛날 기획 -> 삭제
    /// 1. 재료 아이템 프리팹을 인스턴스화
    /// 2. 재료의 개수와 아이디를 설정
    /// countArray[level].countArray[i] 이거 뭘 의미하는거지
    /// </summary>
    public void ResetCanUpgrade(){
        for(int i=0; i<ingredientsCounts[level]; i++){
            //생기면 IngredientsSatisfy가 실행됨. 여기서는 할 필요가 없음.
            GameObject genIngredient = Instantiate(ingredientGameobject, new Vector3(0, 0, 0), Quaternion.identity);
            genIngredient.transform.parent = group.transform;
            //여기서 아이디와 갯수를 지정해줘야함.
            IngredientCheck genIngredientCheck = genIngredient.GetComponent<IngredientCheck>();
            genIngredientCheck.ingredCount = countArray[level].CountArray[i];
            genIngredientCheck.ingredId = IdArray[level].IdArray[i];
        }
        ingredients = gameObject.GetComponentsInChildren<IngredientCheck>();
    }

    /// <summary>
    /// 필요함.
    /// ingredients -> IngredientCheck 형
    /// 한 번에 확인을 해서 하나라도 만족하지 않으면 Can UPgrade = false
    /// isSatisfy : 개별 만족 확인, canUpgrade : 전체 만족 확인
    /// </summary>
    public void IngredientsSatisfy(){
        //Debug.Log("ingredients길이: " + ingredients.Length);
        for(int i=0; i<ingredients.Length; i++){        //갯수만큼 돌려서 만족하지 안으면 업그레이드 못하게 함.
            bool check = ingredients[i].isSatisfy;

            if(check == false){
                canUpgrade = false;
            }
        }
    }

    // 확인해보고 삭제
    // 기존 기획에서 업그레이드를 할 때 아래 재료들을 삭제하는 메서드
    public void DestroyChild_Group_AndReset(){
        for(int i=group.transform.childCount-1; i>=0; --i){
            GameObject.Destroy(group.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 재료의 개수에 따른 group 배치
    /// </summary>
    /// <param name="count">재료의 개수</param>
    private void ReadjustGrid(int count){
        if(count == 1){
            group.spacing = new Vector2 (0, 0);
        }else if (count == 2){
            group.spacing = new Vector2 (100, 0);
        }else if (count == 3){
            group.spacing = new Vector2 (75, 0);
        }else if (count == 4){
            group.spacing = new Vector2 (50, 0);
        }
    }
    
    /// <summary>
    /// 재료가 충족된 상황에서 연구를 클릭했을 때 실행되는 메서드
    /// </summary>
    //시간 단축을 하려면 밑의 함수 복사해서 고쳐라는 부분만 고치면 됨.
    public void SatisfyCheck_FarmDecrease(){              //NOTE : 농장 연구하기 클릭 시 실행되는 함수. 
        if(canUpgrade){
            for(int i=0; i<ingredients.Length; i++){        //재료 삭제 및 글자 색과 딤드 처리 변경
                ingredients[i].RemoveIngredients();         //재료 삭제함.
            }

            // 딤 처리
            BtnUpgradeLevel.LevelUp();                      //버튼 레벨업.

            if(BtnUpgradeLevel.nextUp2 == null){            //첫번째가 아니라면 - 첫번째면 이 변수가 null이 아님.  이걸 레벨별로 나눠야할것 같음;;
                BtnUpgradeLevel.nextUp1.buttonLevelCheck_Dim();
            }else
            {
                BtnUpgradeLevel.nextUp1.buttonLevelCheck_Dim();
                BtnUpgradeLevel.nextUp2.buttonLevelCheck_Dim();
            }
            // 여기까지
            
            //죽임
            SetLevelAndBtn();
            //살림
            DestroyChild_Group_AndReset();                  //image prefab 삭제 함수
            //죽임
            ResetCanUpgrade();                              //canUpgrade변수 초기화. 이것의 위치를 바꿔야할 것 같음.
            // 기존 기획, 업그레이드에 따라 재료 그리드를 다시 설정할 필요가 없으니 삭제해도 무방.
            ReadjustGrid(ingredientsCounts[level]);         //Grid 조정.

            for(int i=0; i<ingredients.Length; i++){
                ingredients[i].SetStringAndColor();         //재료 갯수 표시 & 색 변경
            }

            // labButtonAction 에서 모든 기능 수행 -> Don't Destroy 설정 되어있을 수 있음
            labButtonAction.FarmDecrease();               //농작물 생산시간 감소 -> 여기 있는 함수만 변경하면 됌.

            // 기존 기획, 없어도 됨 -> 확인해볼 것.
            upgradeBtnLevel.Reset_Texts();                 
        }
    }

    public void SatisfyCheck_FishDecrease(){              //NOTE : 물고기 연구하기 클릭 시 실행되는 함수. 
        if(canUpgrade){
            for(int i=0; i<ingredients.Length; i++){        //재료 삭제 및 글자 색과 딤드 처리 변경
                ingredients[i].RemoveIngredients();         //재료 삭제함.
            }

            BtnUpgradeLevel.LevelUp();                      //버튼 레벨업.

            if(BtnUpgradeLevel.nextUp2 == null){            //첫번째가 아니라면 - 첫번째면 이 변수가 null이 아님.
                BtnUpgradeLevel.nextUp1.buttonLevelCheck_Dim();
            }else
            {
                BtnUpgradeLevel.nextUp1.buttonLevelCheck_Dim();
                BtnUpgradeLevel.nextUp2.buttonLevelCheck_Dim();
            }

            SetLevelAndBtn();
            DestroyChild_Group_AndReset();                  //image prefab 삭제 함수
            ResetCanUpgrade();                              //canUpgrade변수 초기화.
            Debug.Log("레벨: " + ingredientsCounts[level]);
            ReadjustGrid(ingredientsCounts[level]);         //Grid 조정.

            for(int i=0; i<ingredients.Length; i++){
                ingredients[i].SetStringAndColor();         //재료 갯수 표시 & 색 변경
            }

            labButtonAction.FishDecrease();               //물고기 생산시간 감소 -> 여기 있는 함수만 변경하면 됌.

            upgradeBtnLevel.Reset_Texts();                  
        }
    }
    public void SatisfyCheck_DduduOutgoingDecrease(){              //NOTE : 활발한 뚜두 연구하기 클릭 시 실행되는 함수. 
        if(canUpgrade){
            for(int i=0; i<ingredients.Length; i++){        //재료 삭제 및 글자 색과 딤드 처리 변경
                ingredients[i].RemoveIngredients();         //재료 삭제함.
            }

            BtnUpgradeLevel.LevelUp();                      //버튼 레벨업.

            if(BtnUpgradeLevel.nextUp2 == null){            //첫번째가 아니라면 - 첫번째면 이 변수가 null이 아님.
                BtnUpgradeLevel.nextUp1.buttonLevelCheck_Dim();
            }else
            {
                BtnUpgradeLevel.nextUp1.buttonLevelCheck_Dim();
                BtnUpgradeLevel.nextUp2.buttonLevelCheck_Dim();
            }

            SetLevelAndBtn();
            DestroyChild_Group_AndReset();                  //image prefab 삭제 함수
            ResetCanUpgrade();                              //canUpgrade변수 초기화.
            Debug.Log("레벨: " + ingredientsCounts[level]);
            ReadjustGrid(ingredientsCounts[level]);         //Grid 조정.

            for(int i=0; i<ingredients.Length; i++){
                ingredients[i].SetStringAndColor();         //재료 갯수 표시 & 색 변경
            }

            labButtonAction.DuduOutgoingDecrease();               //물고기 생산시간 감소 -> 여기 있는 함수만 변경하면 됌.

            upgradeBtnLevel.Reset_Texts();                  
        }
    }
    public void SatisfyCheck_DduduMakerDecrease(){              //NOTE : 뚜두 메이커 연구하기 클릭 시 실행되는 함수. 
        if(canUpgrade){
            for(int i=0; i<ingredients.Length; i++){        //재료 삭제 및 글자 색과 딤드 처리 변경
                ingredients[i].RemoveIngredients();         //재료 삭제함.
            }

            BtnUpgradeLevel.LevelUp();                      //버튼 레벨업.

            if(BtnUpgradeLevel.nextUp2 == null){            //첫번째가 아니라면 - 첫번째면 이 변수가 null이 아님.
                BtnUpgradeLevel.nextUp1.buttonLevelCheck_Dim();
            }else
            {
                BtnUpgradeLevel.nextUp1.buttonLevelCheck_Dim();
                BtnUpgradeLevel.nextUp2.buttonLevelCheck_Dim();
            }

            SetLevelAndBtn();
            DestroyChild_Group_AndReset();                  //image prefab 삭제 함수
            ResetCanUpgrade();                              //canUpgrade변수 초기화.
            Debug.Log("레벨: " + ingredientsCounts[level]);
            ReadjustGrid(ingredientsCounts[level]);         //Grid 조정.

            for(int i=0; i<ingredients.Length; i++){
                ingredients[i].SetStringAndColor();         //재료 갯수 표시 & 색 변경
            }

            labButtonAction.DduduMakerDecrease();               //물고기 생산시간 감소 -> 여기 있는 함수만 변경하면 됌.

            upgradeBtnLevel.Reset_Texts();                  
        }
    }

}