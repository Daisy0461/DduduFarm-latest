using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        if (0 <= hour && hour < 6)
            tempTime = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");
        else if (6 <= hour && hour < 12)
            tempTime = new DateTime(today.Year, today.Month, today.Day, 6, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");
        else if (12 <= hour && hour < 18)
            tempTime = new DateTime(today.Year, today.Month, today.Day, 12, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");    
        else
            tempTime = new DateTime(today.Year, today.Month, today.Day, 18, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");

        if (tempTime != EncryptedPlayerPrefs.GetString("ShopUpdateTime"))
        {
            EncryptedPlayerPrefs.SetString("ShopUpdateTime", tempTime);
            if (priceChangeCallback != null) priceChangeCallback();
        }
        ItemManager.Instance.Save();

        /* test */
        hour += 1;
        hour %= 24;
        /* test */
    }
}
