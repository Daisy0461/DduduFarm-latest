using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip gemSound;
    [SerializeField] private EarnAnim anim;
    
    private ItemManager IM;
    private Ddudu parent; 
    private int gemID;
    private int gemAmount;

    private void Start() 
    {
        IM = ItemManager.Instance;
        parent = GetComponentInParent<Ddudu>();
        gemID = parent.data.info.code + (int)DataTable.Gem - (int)DataTable.Ddudu;
    }

    public void SetEarnGem(int fishCode)
    {
        gemAmount = FishManager.Instance.GetInfo(fishCode).gemCount;
        this.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData e)
    {
        parent.audioSource.PlayOneShot(gemSound);

        // 인벤토리
        if (IM.AddData(gemID, gemAmount) == false)
            return;
        
        // 객체 비활성화
        StartCoroutine(GameObejctSetActiveFalse());
    }

    IEnumerator GameObejctSetActiveFalse()
    {
        yield return null;
        
        this.gameObject.SetActive(false);

        anim.SetText($"+{gemAmount}");
        anim.gameObject.SetActive(true);
    }
}
