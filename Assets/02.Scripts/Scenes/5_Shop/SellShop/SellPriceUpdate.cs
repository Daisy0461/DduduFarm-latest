using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR

using UnityEditor;

#endif // UNITY_EDITOR

public class SellPriceUpdate : MonoBehaviour
{
    [HideInInspector] public int hour;
    [HideInInspector] public int minute;
    [HideInInspector] public int second;
    [HideInInspector] public int millisecond;
    
    [HideInInspector] public string tempTime;

    static public event Action priceChangeCallback = null;
        
    private void OnEnable() 
    {
        UpdateTime();
    }

    public void UpdateTime()
    {
        var today = DateTime.Now;
        
        // hour -> today.Hour
        if (0 <= today.Hour && today.Hour < 6)
            tempTime = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");
        else if (6 <= today.Hour && today.Hour < 12)
            tempTime = new DateTime(today.Year, today.Month, today.Day, 6, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");
        else if (12 <= today.Hour && today.Hour < 18)
            tempTime = new DateTime(today.Year, today.Month, today.Day, 12, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");    
        else
            tempTime = new DateTime(today.Year, today.Month, today.Day, 18, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");

        if (tempTime != EncryptedPlayerPrefs.GetString("ShopUpdateTime"))
        {
            EncryptedPlayerPrefs.SetString("ShopUpdateTime", tempTime);
            if (priceChangeCallback != null) priceChangeCallback();
        }
        ItemManager.Instance.Save();
    }

#if UNITY_EDITOR

    public void OnPriceUpdateButtonClick()
    {
        Debug.Log("Price Update");
        EncryptedPlayerPrefs.SetString("ShopUpdateTime", tempTime);
        if (priceChangeCallback != null) priceChangeCallback();
        ItemManager.Instance.Save();
    }

#endif // UNITY_EDITOR
}

#if UNITY_EDITOR

[CustomEditor(typeof(SellPriceUpdate))]
public class SellPriceUpdateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SellPriceUpdate tg = target as SellPriceUpdate;
        if (GUILayout.Button("Price Update"))
        {
            tg.OnPriceUpdateButtonClick();
        }
    }    
}

#endif // UNITY_EDITOR
