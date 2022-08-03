using UnityEngine;
using UnityEngine.EventSystems;

public class LastCropState : MonoBehaviour, IPointerDownHandler
{ 
    private ItemManager IM;
    private CropManager CM;
    private bool canHarvest = false;    //재배 가능
    // [SerializeField]
    // private GameObject dduduHeart;
    private int cropKind;

    private Vector3 cropPos;

    [SerializeField]
    private Sprite growDoneCropIamge;
    // [SerializeField] private ButtonSound buttonSound;

    void Start(){
        cropPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        cropKind = gameObject.GetComponent<CropGrowTime>().cropKind;
        // buttonSound = FindObjectOfType<ButtonSound>();
        IM = ItemManager.Instance;
        CM = CropManager.Instance;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(canHarvest){
            CropInfo info = CM.GetInfo(cropKind);
            int ran = Random.Range(info.havestMin, info.havestMax+1);
            if (IM.AddData(cropKind, ran) == false)
                return;
            Destroy(gameObject);

            // buttonSound.PlaySound(1);
            FindObjectOfType<ButtonSound>()?.PlaySound(1);
        }
    }

    public void GrowDone(){
        //하트 소환
        // dduduHeart.SetActive(true);
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        sp.sprite = growDoneCropIamge;   
        canHarvest = true;
    }
}
