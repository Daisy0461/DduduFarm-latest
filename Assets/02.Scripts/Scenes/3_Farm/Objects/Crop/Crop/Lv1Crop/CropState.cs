using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CropState : MonoBehaviour, IPointerDownHandler
{ 
    [SerializeField] private GameObject nextCrop;
    
    public GameObject growDoneImage;

    private bool canHarvest = false;    //재배 가능
    private Vector3 cropPos;

    void Start()
    {
        cropPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        TouchManager.ZoomAmountChange += this.IconSizeChange;
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        if (canHarvest)
        {     
            CropGrowSound cgs = nextCrop.GetComponent<CropGrowSound>();
            cgs.AudioAwakePlay();
            if(cgs == null) 
            {
                Debug.LogError("cgs = null");
                return;
            }

            InstantiateNextCrop();
            cgs.AudioAwakePlayFalse();
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void InstantiateNextCrop()
    {
        var cropGrowTime = Instantiate(nextCrop, cropPos, Quaternion.identity).GetComponent<CropGrowTime>();        //지금은 색을 바꾼 Crop이 나오는데 이걸 nextCrop을 바꿔주면 해당 nextCrop이 나옴.
        cropGrowTime.SetRechargeScheduler(true);
    }

    public void GrowDone()
    {
        growDoneImage.SetActive(true);
        canHarvest = true;
    }

    public GameObject GetNextCrop()
    {
        return nextCrop;
    }

    public void IconSizeChange(float zoomAmount)
    {
        growDoneImage.transform.localScale = Vector3.one * zoomAmount * 0.1f;
    }

    private void OnDestroy() 
    {
        TouchManager.ZoomAmountChange -= this.IconSizeChange;
    }

}
