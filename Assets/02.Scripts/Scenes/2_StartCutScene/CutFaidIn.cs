using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutFaidIn : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer cutImage;
    private Color cutImageColor;
    // Start is called before the first frame update
    void OnEnable()
    {
        cutImageColor = cutImage.color;
        cutImageColor.a = 0.0f;
        cutImage.color = cutImageColor;
    }

    // Update is called once per frame
    void Update()
    {
        if(cutImageColor.a < 1.0){        //alpha가 255보다 작을 때 - 더 진해질 수 있을 때 더 진하게 만듦.
            cutImageColor.a = cutImageColor.a + 0.03f;
            cutImage.color = cutImageColor;
            //Debug.Log("alpha = " + cutImageColor.a);
        }else{
            cutImageColor.a = 1.0f;
            cutImage.color = cutImageColor;
        }
    }
}
