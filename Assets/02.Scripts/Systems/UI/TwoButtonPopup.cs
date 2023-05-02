using System;
using UnityEngine;
using UnityEngine.UI;

public class TwoButtonPopup : MonoBehaviour
{
    [Header("Two Button Popup")] [Space(8)]
    [SerializeField] private Text _commentText;

    protected Action _onFirstAction = null;
    protected Action _onSecondAction = null;
    protected Action _onFirstCloseAction = null;
    protected Action _onCloseAction = null;

    public virtual void Activate(params object[] objs)
    {
        _commentText.text = (string)objs[0];
        this.gameObject.SetActive(true);
    }

    public void SetAction(Action firstAction = null, Action secondAction = null)
    {
        _onFirstAction = firstAction;
        _onSecondAction = secondAction;
    }

    public void SetOnCloseAction(Action closeAction)
    {
        _onCloseAction = closeAction;
    }

    public void SetOnFirstCloseAction(Action firstAction)
    {
        _onFirstCloseAction = firstAction;
    }

    public void OnFirstButtonClick()
    {
        _onFirstAction?.Invoke();
        _onFirstCloseAction?.Invoke();
        OnCloseButtonClick();
    }

    public void OnSecondButtonClick()
    {
        _onSecondAction?.Invoke();
        OnCloseButtonClick();
    }

    public void OnCloseButtonClick()
    {
        _onCloseAction?.Invoke();
        this.gameObject.SetActive(false);
    }
}
