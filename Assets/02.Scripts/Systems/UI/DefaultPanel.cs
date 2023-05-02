using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DefaultPanel : MonoBehaviour
{
    [Header("UI Components")][Space(5)]
    [SerializeField] protected Text[] _titleText;
    [SerializeField] protected Text[] _text;
    [SerializeField] protected Image[] _images;

    [HideInInspector] protected UIItem[] uiItems;

    protected Action _onCloseAction = null;

    public virtual void Activate(params object[] objs)
    {
        this.gameObject.SetActive(true);
    }

    public void SetOnCloseAction(Action action)
    {
        _onCloseAction = action;
    }

    public void AddOnCloseAction(Action action)
    {
        _onCloseAction += action;
    }

    public virtual void OnCloseButtonClick()
    {
        _onCloseAction?.Invoke();
        this.gameObject.SetActive(false);
    }
}
