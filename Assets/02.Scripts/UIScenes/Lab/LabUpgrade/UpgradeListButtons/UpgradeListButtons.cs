using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� : ��ųƮ���� �� �����µ� ���� ��ư���� ȸ������ ���ͼ�, ù��° ��ư�� �ν����Ϳ��� �־ ���� �� �ٷ� Ȱ��ȭ�� �ǵ��� ���� �ڵ�.
/// </summary>
public class UpgradeListButtons : MonoBehaviour
{
    [SerializeField]
    private LabListButtonAction fristButton;

    public void FirstClickUpgradeButton(){
        fristButton.FristOpenUpgradeList();  
    }
}
