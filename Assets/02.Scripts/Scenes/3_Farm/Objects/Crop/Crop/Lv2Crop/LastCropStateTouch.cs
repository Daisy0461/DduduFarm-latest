using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LastCropStateTouch : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private LastCropState parentLastCropState;
    private int cropKind;

    private CropManager CM;
    private ItemManager IM;

    void Start()
    {
        cropKind = parentLastCropState.GetCropKind();
        CM = CropManager.Instance;
        IM = ItemManager.Instance;
    }

    public void OnPointerDown(PointerEventData eventData) 
    {
        CropInfo info = CM.GetInfo(cropKind);
        int ran = Random.Range(info.havestMin, info.havestMax+1);
        if (IM.AddData(cropKind, ran) == false) return;
        parentLastCropState.DestroyObject();
        FindObjectOfType<ButtonSound>()?.PlaySound(1);
    }
}
