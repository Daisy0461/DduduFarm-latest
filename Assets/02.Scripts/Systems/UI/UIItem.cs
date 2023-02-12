using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIItem : MonoBehaviour
{
    public Action<int> _onClickAction;
    
    private int id;

    protected virtual void Awake() 
    {
        id = transform.GetSiblingIndex();
    }

    public virtual void SetOnClickAction(Action<int> action)
    {
        _onClickAction = action;
    }

    public virtual void OnButtonClick()
    {
        _onClickAction?.Invoke(id);
    }
}
