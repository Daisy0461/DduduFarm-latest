using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CropButtonColorChange : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private Color originColor;
    public GameObject checkButton;

    void Start(){
        originColor = image.color;
    }

    public void ChangeButtonColor(){      //이걸 사용하려면 파라미터를 계속 바꿔줘야 가능함.
        if(checkButton == this.gameObject){
            image.color = new Color(0.5f, 1, 0);
        }else{
            image.color = originColor;
        }
    }
}
