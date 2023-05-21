using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : SingletonBase<PopupManager>
{
    [SerializeField] private DefaultPanel _errorPopup;
    
    public DefaultPanel GetErrorPopup()
    {
        return _errorPopup;
    }
}
