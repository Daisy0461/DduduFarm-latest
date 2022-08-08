using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CropStateTocuh : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private CropState parentCropState;        //작물 넣으면 됌.
    private CropGrowSound cgs;


    void Start()
    {
        cgs = parentCropState.GetNextCrop().GetComponent<CropGrowSound>();
    }

    public void OnPointerDown(PointerEventData eventData) {    
        cgs.AudioAwakePlay();
        if(cgs == null) Debug.Log("cgs = null");
        parentCropState.InstantiateNextCrop();
        //여기서 cropAudio에 접근해서 Play하면 될듯
        //CropGrowSound cgs = nextCrop.GetComponent<CropGrowSound>(); 
        cgs.AudioAwakePlayFalse();

        parentCropState.DestroyObject();
    }
}
