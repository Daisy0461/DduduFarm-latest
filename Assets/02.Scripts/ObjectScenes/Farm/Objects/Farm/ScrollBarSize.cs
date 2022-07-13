using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarSize : MonoBehaviour
{
    public float barSize = 0.2f;
    void OnEnable(){
        Invoke("Size", barSize);
    }

    void Size(){
        transform.GetComponent<Scrollbar>().size = barSize;
    }
}
