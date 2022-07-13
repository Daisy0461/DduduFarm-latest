using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabListButtonAction : MonoBehaviour
{
    [SerializeField]
    // 활성화 되어있는 연구 종류 버튼
    private GameObject activeGameObject;        //활성화할 페이지 넣으면 됌.
    [SerializeField]
    // 다른 연구 종류 버튼
    private LabListButtonAction[] otherButtons;     //다른 버튼들
    [SerializeField]
    // 자기 자신의 버튼 이미지
    private Image image;
    // 원래 컬러 : 선택할 때 Dim 처리하려고 있는 변수
    private Color originColor;

    void Start(){
        originColor = Color.white;
    }

    /// <summary>
    /// 1. 처음에 들어갈 때 activeGameObject만 활성화하고 다른 버튼은 회색 처리
    /// </summary>
    public void FristOpenUpgradeList(){                 //투명하게 나오는걸 방지하기 위해 실행함.
        for(int i=0; i<otherButtons.Length; i++){       //다른 버튼들 색 회색으로 만듦.
            otherButtons[i].SetGrey();
        }
        activeGameObject.SetActive(true);
    }
    
    /// <summary>
    /// 1. 특정 버튼이 선택되면, 자신은 원래 색으로 바꾸고, 다른 버튼을 회색으로 처리
    /// </summary>
    public void SelectThisUpgrade(){
        for(int i=0; i<otherButtons.Length; i++){       //다른 버튼들 색 회색으로 만듦.
            otherButtons[i].SetGrey();
        }
        image.color = originColor;                      //자신은 원래 색으로 돌려보냄.  이걸 UpgradeListButton에서 실행시키면 originColor가 없는 상태여서 투명하게 나옴.
        activeGameObject.SetActive(true);
    }


    /// <summary>
    /// 1. 이미지 색 바꾸기
    /// 2. 비활성화
    /// </summary>
    public void SetGrey(){
        image.color = Color.grey;
        activeGameObject.SetActive(false);
    }

}
