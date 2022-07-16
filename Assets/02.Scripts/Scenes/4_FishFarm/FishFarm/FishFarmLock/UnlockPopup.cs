using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnlockPopup : MonoBehaviour, IPointerDownHandler
{
    public GameObject lockPopup;

    public void OnPointerDown(PointerEventData eventData) 
    {
        lockPopup.SetActive(true);
    }
}
