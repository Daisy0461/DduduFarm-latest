using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSelectListGenerator : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject activeParent;
    [SerializeField] private TextObject[] fishSlots;
    [SerializeField] ItemManager IM;
    [SerializeField] FishManager FM;

    public Ddudu selectedDdudu;

    private void OnEnable() 
    {
        IM = ItemManager.Instance;
        FM = FishManager.Instance;
        ActiveFishSlot();
    }

    void ActiveFishSlot()
    {
        for(int id=0; id<fishSlots.Length; id++)
        {
            var fish = IM.GetData(id + (int)DataTable.Fish + 1);
            if (fish != null)
            {
                fishSlots[id].gameObject.SetActive(true);
                fishSlots[id].contentText.text = fish.amount.ToString();
            }
            else fishSlots[id].gameObject.SetActive(false);
        }
    }

    public void OnclickFishSlot(int id)
    {
        audioSource.Play();
        FeedDdudu(id);
        activeParent.SetActive(false);
    }

    public void FeedDdudu(int id)
    {
        // 먹이주기
        selectedDdudu.data.satiety = selectedDdudu.data.info.maxFull;
        
        selectedDdudu.data.like += FM.GetInfo(id).like;
        if (selectedDdudu.data.like >= selectedDdudu.data.info.maxFull)
        {
            //  뚜두의 선물 - 한 종 당 하나씩
        }
        selectedDdudu.EatProcess(id);
        
        // 물고기 amount 차감
        IM.RemoveData(id);
    }
}
