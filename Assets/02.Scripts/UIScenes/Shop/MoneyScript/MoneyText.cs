using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
    [SerializeField]
    private Text moneyText;
    void OnEnable(){
        moneyText.text = string.Format("{0:#,##0}", ItemManager.Instance.GetData((int)DataTable.Money)?.amount ?? 0);
    }

    public void TextUpdate()
    {
        moneyText.text = string.Format("{0:#,##0}", ItemManager.Instance.GetData((int)DataTable.Money)?.amount ?? 0);
    }

    //Text 변경 메서드
    static public void ChangeMoneyText(Text txt, int money)
    {
        txt.text = string.Format("{0:#,##0}", money);
    }
}
