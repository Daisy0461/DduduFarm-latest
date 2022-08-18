using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gem : MonoBehaviour, IPointerClickHandler
{
    public GameObject anim;
    [SerializeField] AudioClip gemSound;

    ItemManager IM;
    Ddudu parent;
    
    int gemID;
    int gemAmount;

    private void Start() 
    {
        IM = ItemManager.Instance;
        parent = GetComponentInParent<Ddudu>();
        gemID = parent.data.info.code + (int)DataTable.Gem - (int)DataTable.Ddudu;
        gemAmount = (parent.data.info.code > (int)DataTable.CombineDdudu) ? 2 : 1;
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
        yield return new WaitForFixedUpdate();
        
        this.gameObject.SetActive(false);
        anim.SetActive(true);
    }
}
