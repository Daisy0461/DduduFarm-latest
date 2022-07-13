using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortingDropdown : MonoBehaviour
{
    public Inventory inventory;
    public Dropdown dropdown;
    public Text label;
    string[] option = new string[]{"획득순", "최신순"};
    ItemManager IM;

    private void Start() 
    {
        IM = ItemManager.Instance;

        SetFunction_UI();
    }

    private void SetFunction_UI()
    {
        ResetFunction_UI();

        dropdown.onValueChanged.AddListener(delegate {
            Function_Dropdown(dropdown);
        });
    }

    private void ResetFunction_UI()
    {
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.options.Clear();

        for (int i = 0; i < option.Length; i++)
        {
            Dropdown.OptionData newData = new Dropdown.OptionData();
            newData.text = option[i];
            dropdown.options.Add(newData);
        }
        dropdown.SetValueWithoutNotify(-1);
        dropdown.SetValueWithoutNotify(0);
    }

    private void Function_Dropdown(Dropdown select)
    {
        string op = select.options[select.value].text;
        if (op == option[0])
        {
            OnClickSortObtainFirst();
        }
        else if (op == option[1])
        {
            OnClickSortLatestFirst();
        }
        inventory.Show();
        // label.text = "정렬 기준";
    }

    public void OnClickSortLatestFirst()
    {
        // 최신순 : 가장 최근에 획득한 것이 상위에 노출
        Debug.Log("최신순 : 가장 최근에 획득한 것이 상위에 노출");
        inventory.sortStatus = "Latest";
    }

    public void OnClickSortObtainFirst()
    {
        // 획득순 : 가장 처음 획득한 것이 제일 위에 노출
        Debug.Log("획득순: 가장 처음 획득한 것이 제일 위에 노출");
        inventory.sortStatus = "Obtain";
        
    }
}
