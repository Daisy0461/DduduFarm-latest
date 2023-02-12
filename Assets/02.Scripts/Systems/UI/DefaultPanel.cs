using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPanel : MonoBehaviour
{
    [SerializeField] private UIItem[] _uiItem;

    protected virtual void Awake() 
    {
        foreach (var item in _uiItem)
        {
            item.SetOnClickAction(OnButtonClick);
        }
    }

    public virtual void OnButtonClick(int index)
    {
        
    }
}
