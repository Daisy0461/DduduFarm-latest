using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CutSceneText : MonoBehaviour
{
    private Text text;
    public string outputText;
    public float textTime;

    void OnEnable(){
        text = GetComponent<Text>();
        text.DOText(outputText, textTime);
    }
}
