using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaboratoryTest : MonoBehaviour
{
    [SerializeField] private GameObject _testObject;

#if UNITY_EDITOR
    
    private void Start() 
    {
        _testObject.SetActive(true);
    }

    public void ResearchAllItem()
    {
        var researchItems = ResearchManager.Instance.GetInfoEnumerable();
        foreach (var item in researchItems)
        {
            if (!item.researchable || ResearchManager.Instance.IsLastResearchItem(item.code)) continue;
            ResearchManager.Instance.Research(item.code);
        }
    }

    public void AddAllGemToInventory10()
    {
        for (int itemId = 701; itemId < 711; ++itemId)
        {
            ItemManager.Instance.AddData(itemId, 1000);
        }

        Debug.Log("���� 1000���� �߰� �Ϸ�");
    }

    public void ResetInventory()
    {
        
        ItemManager.Instance.ResetDataList();
        
        Debug.Log("�κ��丮 �ʱ�ȭ �Ϸ�");
    }

    public void PrintInventoryStatus()
    {
        if (ItemManager.Instance.GetDataListCount() != 0)
            ItemManager.Instance.GetDataList().ForEach(i => Debug.Log(i.id + ":" + i.info.name + ":" + i.info.note + ":" + i.info.imgPath + ":" + i.obtainDate + ":" + i.amount));

        else
            Debug.Log("�κ��丮�� �������� ����");
    }

    public void ResetResearch()
    {
        ResearchManager.Instance.SetDataListClear();

        GameObject[] researchItemUIObjectList = GameObject.FindGameObjectsWithTag("ResearchItem");

        Debug.Log("���� �ʱ�ȭ �Ϸ�");
    }

    public void PrintResearchStatus()
    {
        Debug.Log("����� ���� ����");
    }

#endif 
}
