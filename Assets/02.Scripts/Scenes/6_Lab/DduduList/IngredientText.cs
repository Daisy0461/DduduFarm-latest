using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 한나한테 물어보고 삭제 -> 기본 뚜두 부르기 부분.
/// </summary>
public class IngredientText : MonoBehaviour
{
    ItemManager IM;
    private Text text;
    [SerializeField]
    private int needIngred;      //필요한 재료의 양.
    [SerializeField]
    private int ingredId;   //재료의 ID를 씀.
    public bool isSatisfy = false;

    void Start()
    {
        IM = ItemManager.Instance;

        int getHaveIngred = 0;
        if (IM.IsDataExist(ingredId))
            getHaveIngred = IM.GetData(ingredId).amount;

        text = gameObject.GetComponent<Text>();
        text.text = getHaveIngred.ToString()+"/"+needIngred.ToString();

        if(needIngred > getHaveIngred) 
        {     //재료가 부족하다.
            text.color = Color.red;
        } 
        else 
        {          //재료 충분
            text.color = Color.green;
            isSatisfy = true;
        }
    }
}
