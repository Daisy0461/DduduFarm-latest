using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이유 : 스킬트리는 다 나오는데 위에 버튼들이 회색으로 나와서, 첫번째 버튼을 인스펙터에서 넣어서 실행 시 바로 활성화가 되도록 만든 코드.
/// </summary>
public class UpgradeListButtons : MonoBehaviour
{
    [SerializeField]
    private LabListButtonAction fristButton;

    public void FirstClickUpgradeButton(){
        fristButton.FristOpenUpgradeList();  
    }
}
