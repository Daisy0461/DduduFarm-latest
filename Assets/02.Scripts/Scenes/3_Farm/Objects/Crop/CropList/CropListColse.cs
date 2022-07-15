using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropListColse : MonoBehaviour
{
    [SerializeField]
    private FarmFind farmFind;

    public void CropListUI_Colse(GameObject cropList){
        cropList.SetActive(false);
        for(int i = 0; i<farmFind.farmStateList. Length; i++){
            farmFind.farmStateList[i].ReturnColor();
            farmFind.farmStateList[i].afterPushFunc = false;
        }
    }
}
