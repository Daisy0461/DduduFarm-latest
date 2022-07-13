using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CropState : MonoBehaviour, IPointerDownHandler
{ 
    public GameObject growDoneImage;

    private bool canHarvest = false;    //재배 가능
    [SerializeField]
    private GameObject nextCrop;

    [SerializeField]
    private GameObject min, sec, divide;

    private Vector3 cropPos;
    private CropGrowTime cropGrowTime;
    void Start(){
        cropPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        //min.transform.parent = gameObject.transform; sec.transform.parent = gameObject.transform; divide.transform.parent = gameObject.transform;
        cropGrowTime = gameObject.GetComponent<CropGrowTime>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(canHarvest){     
            CropGrowSound cgs = nextCrop.GetComponent<CropGrowSound>();
            cgs.AudioAwakePlay();
            if(cgs == null) Debug.Log("cgs = null");
            Instantiate(nextCrop, cropPos, Quaternion.identity);        //지금은 색을 바꾼 Crop이 나오는데 이걸 nextCrop을 바꿔주면 해당 nextCrop이 나옴.
            //여기서 cropAudio에 접근해서 Play하면 될듯
            //CropGrowSound cgs = nextCrop.GetComponent<CropGrowSound>(); 
            cgs.AudioAwakePlayFalse();

            if(min != null && sec != null && divide != null) {  //차일드화 해제한 글자 삭제.
                //Debug.Log("1번 작물");
                Destroy(min); Destroy(sec); Destroy(divide);
            }
            Destroy(gameObject); //된다. -> 그냥 삭제 잘됌.
        }
    }

    public void GrowDone(){
        min.transform.parent = gameObject.transform; sec.transform.parent = gameObject.transform; divide.transform.parent = gameObject.transform;
        //GameObject.Instantiate(growDoneImage, cropPos + new Vector3 (-0.05f, -0.55f, 0), Quaternion.identity).transform.parent = this.transform;
        growDoneImage.SetActive(true);
        canHarvest = true;
    }

    public GameObject GetNextCrop(){
        return nextCrop;
    }
    public Vector3 GetCropPos(){
        return cropPos;
    }
}
