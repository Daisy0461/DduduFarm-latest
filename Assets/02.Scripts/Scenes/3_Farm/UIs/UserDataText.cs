using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserDataText : MonoBehaviour
{
    [SerializeField] private Text loveText;
    public Text dduduText;
    public Text moneyText;
    ItemManager IM;

    void Start()
    {
        IM = ItemManager.Instance;
        loveText.text = string.Format("{0:#,##0}", IM.GetData((int)DataTable.Love)?.amount ?? 0);
        dduduText.text = string.Format("{0:#,##0}", DduduManager.Instance.GetDataListCount());
        moneyText.text = string.Format("{0:#,##0}", IM.GetData((int)DataTable.Money)?.amount ?? 0);
    }

    public void RenewText(Text text, int value)
    {
        text.text = string.Format("{0:#,##0}", value);
    }
}
