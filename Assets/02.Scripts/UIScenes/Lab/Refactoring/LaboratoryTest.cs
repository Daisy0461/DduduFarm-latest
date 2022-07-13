using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryTest : MonoBehaviour
{
    public void AddAllGemToInventory10()
    {
        for (int itemId = 701; itemId < 711; ++itemId)
        {
            ItemManager.Instance.AddData(itemId, 10);
        }

        Debug.Log("보석 10개씩 추가 완료");
    }

    public void ResetInventory()
    {
        
        ItemManager.Instance.ResetDataList();
        
        Debug.Log("인벤토리 초기화 완료");
    }

    public void PrintInventoryStatus()
    {
        if (ItemManager.Instance.GetDataListCount() != 0)
            ItemManager.Instance.GetDataList().ForEach(i => Debug.Log(i.id + ":" + i.info.name + ":" + i.info.note + ":" + i.info.imgPath + ":" + i.obtainDate + ":" + i.amount));

        else
            Debug.Log("인벤토리에 아이템이 없음");
    }

    public void ResetResearch()
    {
        ResearchManager.Instance.SetDataListClear();

        GameObject[] researchItemUIObjectList = GameObject.FindGameObjectsWithTag("ResearchItem");

        foreach (GameObject researchItemUIObject in researchItemUIObjectList)
        {
            researchItemUIObject.GetComponent<ResearchItemUI>().UpdateResearchItemUI();
        }

        Debug.Log("연구 초기화 완료");
    }

    public void PrintResearchStatus()
    {
        if(ResearchManager.Instance.GetDataListCount() != 0)
        {
            foreach (var data in ResearchManager.Instance.GetDataList())
            {
                if(data.isPrime)
                    Debug.LogFormat("{0} 연구 레벨 : {1}", data.info.name, data.info.level.ToString());
            }
        }

        else
            Debug.Log("진행된 연구 없음");
    }
}
