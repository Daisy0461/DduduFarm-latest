using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// 1. 재료의 만족 여부 체크
/// 2. 만족 여부로 텍스트 색을 변경
/// 3. 재료의 이미지 관리.
/// </summary>
public class IngredientCheck : MonoBehaviour
{
    //아 추가적으로 ID 이용해서 이미지도 들고오는거 같던데 이것도 나중에 시험끝나고 나서 물어봐야겠다.  
    //각각 Ingredient 이미지에 들어있는 스크립트임.
    ItemManager IM;
    //[HideInInspector]
    public int ingredCount;        //필요한 재료의 양을 지정함.
    //[HideInInspector]
    public int ingredId;
    public Text countText;
    public bool isSatisfy = false;
    
    [SerializeField]
    private Image ingredientImage;
    private SatisfyCheck parentComponent;

    private void Awake()
    {
        IM = ItemManager.Instance; 
    }
    
    void Start(){
        SetStringAndColor();
        parentComponent.canUpgrade = true;          //여기에 true를 해도 되는 이유: 어차피 아래 IngredientSatisfy는 모든 값을 검사하기 때문에 괜찮음.
        parentComponent.IngredientsSatisfy();
        //ingredCount, ID는 이미 값이 들어와있는 상태이다.
        ingredientImage.sprite = Resources.Load<Sprite>(ingredientImagePath(ingredId));
    }

    // 인벤토리에서 아이템 빼주는 메서드 -> IM에 있는 걸 호출한다는 느낌.
    public void RemoveIngredients(){
        IM.RemoveData(ingredId, ingredCount);
    }

    /// <summary>
    /// 1. 조건을 만족하는지 체크
    /// 2. 재료 아이템의 아이디로 현재 보유하고 있는 개수 확인
    /// 3. 재료 개수가 충분하다면 isSatisfy를 true로 설정하고 글씨 색 변경
    /// -> 재활용 가능하게 : 모든 재료를 사용하는 부분에서 사용.
    /// </summary>
    public void SetStringAndColor(){        //NOTE : 클릭 시 실행 추가해야함.
        parentComponent = gameObject.GetComponentInParent<SatisfyCheck>();             

        int count = 0;
        if (IM.IsDataExist(ingredId))
            count = IM.GetData(ingredId).amount;

        string countString = count.ToString();
        string requestCount = ingredCount.ToString();

        if(count < ingredCount){        //부족하다.
            countText.color = Color.red;
        }else{      //같거나 더 많다.
            isSatisfy = true;
            countText.color = Color.green;
        }
        countText.text = countString + "/" + requestCount;
    }

    /// <summary>
    /// 1. 아이디를 입력 받고 재료 아이템의 이미지 경로를 반환함.
    /// -> dictionary (한나랑 얘기) -> 시간 복잡도 최적화, 관리 좋음.
    /// </summary>
    /// <param name="id">재료 아이템의 아이디</param>
    /// <returns>이미지 경로</returns>
    private string ingredientImagePath(int id){
        switch(id)
        {
            case 201:
                return "Crop/감자작물3";
            case 202:
                return "Crop/노랑작물3";
            case 203:
                return "Crop/방울작물3";
            case 204:
                return "Crop/분홍작물3";
            case 205:
                return "Crop/포도작물3";
            case 701:
                return "Gem/gem_potato";
            case 702:
                return "Gem/gem_yellow";
            case 703:
                return "Gem/gem_blue";
            case 704:
                return "Gem/gem_pink";
            case 705:
                return "Gem/gem_grape";
            default:
                return "";
        }
    }
}
