using UnityEngine;

public class FishEggListGenerator : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject activeParent;
    [SerializeField] private TextObject[] eggSlots;
    [SerializeField] ItemManager IM;
    [SerializeField] FishCount fishCount;

    private void OnEnable() 
    {
        IM = ItemManager.Instance;
        ActiveEggSlot();
    }

    public void ActiveEggSlot()
    {
        for (int id=0; id<5; id++)
        {
            var fishegg = IM.GetData(id + (int)DataTable.FishEgg + 1);
            if (fishegg != null)
            {
                eggSlots[id].gameObject.SetActive(true);
                eggSlots[id].contentText.text = fishegg.amount.ToString();
            }
            else eggSlots[id].gameObject.SetActive(false);
        }
    }

    public void OnclickEggSlot(int id)  // 물고기 알 아이디
    {
        audioSource.Play();
        fishCount.fishEggId = id;
        fishCount.gameObject.SetActive(true);
        activeParent.SetActive(false);
    }
}
