
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ForestTalkText : MonoBehaviour
{
    public enum TalkType
    {
        Greet,
        Call,
        Combine,
        Remove,
    }

    [SerializeField] private Text _talkText;
    [SerializeField] private string[] _greetStrings;
    [SerializeField] private string[] _callStrings;
    [SerializeField] private string[] _combineStrings;
    [SerializeField] private string[] _removeStrings;

    [SerializeField] private ForestCallDduduPanel _forestCallPanel;
    [SerializeField] private DduduCombinePopup _combinePopup;
    [SerializeField] private TwoButtonPopup _removePopup;

    public void Start()
    {
        EventText(TalkType.Greet);
        _forestCallPanel.AddOnCloseAction(() => {EventText(TalkType.Call);});
        _combinePopup.AddOnCloseAction(() => {EventText(TalkType.Combine);});
        _removePopup.SetOnFirstCloseAction(() => {EventText(TalkType.Remove);});
    }

    public void EventText(TalkType talkType)
    {
        _talkText.text = "";
        _talkText.DOKill();

        var content = string.Empty;
        if (talkType == TalkType.Greet)
        {
            var randomInt = Random.Range(0, _greetStrings.Length);
            content = _greetStrings[randomInt];
        }
        if (talkType == TalkType.Call)
        {
            var randomInt = Random.Range(0, _greetStrings.Length);
            content = _callStrings[randomInt];
        }
        if (talkType == TalkType.Combine)
        {
            var randomInt = Random.Range(0, _greetStrings.Length);
            content = _combineStrings[randomInt];
        }
        if (talkType == TalkType.Remove)
        {
            var randomInt = Random.Range(0, _greetStrings.Length);
            content = _removeStrings[randomInt];
        }
        _talkText.DOText(content, content.Length * 0.1f + 0.3f);
    }
}
