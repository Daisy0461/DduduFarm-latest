using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIItem : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text[] _texts;

    public Action _onClickAction = null;
    public Action _onCloseAction = null;
    
    protected int _code;

    protected virtual void Awake() 
    {
    }

    public virtual void SetData(params object[] objs)
    {
    }

    public void SetSprite(string imgPath)
    {
        _image.sprite = Resources.Load<Sprite>(imgPath);
    }

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string content)
    {
        foreach (var text in _texts)
        {
            text.text = content;
        }
    }

    public virtual void SetOnClickAction(Action action)
    {
        _onClickAction = action;
    }

    public virtual void OnButtonClick()
    {
        _onClickAction?.Invoke();
    }

    public virtual void OnCloseButtonClick()
    {
        _onCloseAction?.Invoke();
        gameObject.SetActive(false);
        
    }
}
