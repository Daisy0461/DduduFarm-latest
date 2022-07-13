using System.Collections;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public GameObject anim;
    DduduGemFeedManage DFM;
    int gemID;
    int gemAmount;

    ItemManager IM;
    Ddudu parent;

    private void Start() 
    {
        IM = ItemManager.Instance;
        DFM = GetComponentInParent<DduduGemFeedManage>();
        parent = GetComponentInParent<Ddudu>();
        gemID = parent.data.info.code + (int)DataTable.Gem - (int)DataTable.Ddudu;
        gemAmount = (parent.data.info.code > (int)DataTable.CombineDdudu) ? 2 : 1;
    }

    public void OnClickGem() 
    {
        DFM.PlayGemSound();

        // 인벤토리
        if (IM.AddData(gemID, gemAmount) == false)
            return;
        
        // 객체 비활성화
        StartCoroutine(GameObejctSetActiveFalse());
    }

    IEnumerator GameObejctSetActiveFalse()
    {
        yield return new WaitForFixedUpdate();
        
        this.gameObject.SetActive(false);
        anim.SetActive(true);
    }
}
