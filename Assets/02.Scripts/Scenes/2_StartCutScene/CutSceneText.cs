using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class CutSceneText : MonoBehaviour
{
    [TextArea]
    public string outputText;
    public float textTime; 

    private Text text;
    private float _textTime => textTime > 0 ? textTime : outputText.Length * 0.1f;

    void OnEnable(){
        text = GetComponent<Text>();
        outputText = outputText.Replace("\\n", "\n");

        var pattern = @"(?<=\[)(.*?)(?=\])";
        outputText = Regex.Replace(outputText, pattern, "");
        
        outputText = outputText.Replace("[]", "");        
    }

    public async Task PlayCutSceneText()
    {
        await text.DOText(outputText, _textTime).AsyncWaitForCompletion();
        await Task.Delay(1000);
        gameObject.SetActive(false);
    }
}
