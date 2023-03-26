using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    [SerializeField] private ForestCallDduduPanel _callDduduPanel;
    public void OnCallDduduButtonClick()
    {
        _callDduduPanel.Activate();
    }
}
