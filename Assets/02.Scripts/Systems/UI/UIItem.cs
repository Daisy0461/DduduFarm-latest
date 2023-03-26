using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIItem : MonoBehaviour
{
    public Action _onClickAction = null;
    public Action _onCloseAction = null;
    
    protected int _code;

    protected virtual void Awake() 
    {
    }

    public virtual void SetData(params object[] objs)
    {
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
