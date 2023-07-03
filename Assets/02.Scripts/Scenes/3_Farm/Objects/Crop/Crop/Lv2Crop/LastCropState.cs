using UnityEngine;
using UnityEngine.EventSystems;

public class LastCropState : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Sprite growDoneCropIamge;
 
    private bool canHarvest = false;    //재배 가능
    private int cropKind;
    private Vector3 cropPos;

    void Start()
    {
        cropPos = gameObject.transform.position;
        cropKind = gameObject.GetComponent<CropGrowTime>().cropKind;
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        if (canHarvest) // 작물 수확
        {
            CropInfo info = CropManager.Instance.GetInfo(cropKind);
            int ran = Random.Range(info.havestMin, info.havestMax+1);
            if (ItemManager.Instance.AddData(cropKind, ran) == false) return;

            DestroyObject();
            FindObjectOfType<ButtonSound>()?.PlaySound(1);
        }
    }

    public void GrowDone()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        sp.sprite = growDoneCropIamge;   
        canHarvest = true;
    }

    public int GetCropKind()
    {
        return cropKind;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
