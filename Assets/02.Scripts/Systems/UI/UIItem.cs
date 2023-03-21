using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIItem : MonoBehaviour
{
    public Action _onClickAction;
    
    private int code;

    protected virtual void Awake() 
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
}
