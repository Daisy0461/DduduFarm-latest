using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CutSceneText : MonoBehaviour
{
    [TextArea]
    public string outputText;
    public float textTime; 

    private Text text;
    private float _textTime => textTime > 0 ? textTime : outputText.Length * 0.2f;

    void OnEnable(){
        text = GetComponent<Text>();
        outputText = outputText.Replace("\\n", "\n");

        var pattern = @"(?<=\[)(.*?)(?=\])";
        outputText = Regex.Replace(outputText, pattern, "");
        
        outputText = outputText.Replace("[]", "");

        text.DOText(outputText, textTime);
    }
}
