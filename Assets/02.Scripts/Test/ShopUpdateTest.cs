using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopUpdateTest : MonoBehaviour
{
    [HideInInspector] public int hour;
    [HideInInspector] public int minute;
    [HideInInspector] public int second;
    [HideInInspector] public int millisecond;
    
    [HideInInspector] public string tempTime;

    static public event Action<float> priceChangeCallback = null;
    float marketPower = 1f;
    float currentPosition = 0f;

        
    private void OnEnable() 
    {
        UpdateTime();
    }

    public void UpdateTime()
    {
        // 시간 갱신
        // 한 번 버튼을 누를 때마다 1시간씩 갱신
        // 00, 06, 12, 18 시(KST) 일 때 가격 갱신
        // 이미 갱신했으면 갱신하지 않기
        // EncryptedPlayerPrefs.SetFloat("ShopUpdateTime", )
        var today = DateTime.Now;

        if (0 <= hour && hour < 6)
        {
            tempTime = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");
        }
        else if (6 <= hour && hour < 12)
        {
            tempTime = new DateTime(today.Year, today.Month, today.Day, 6, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");
        }
        else if (12 <= hour && hour < 18)
        {
            tempTime = new DateTime(today.Year, today.Month, today.Day, 12, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");    
        }
        else
        {
            tempTime = new DateTime(today.Year, today.Month, today.Day, 18, 0, 0, 0).ToString("yyyy-MM-dd-HH:mm:ss fff");
        }

        if (tempTime != EncryptedPlayerPrefs.GetString("ShopUpdateTime"))
        {
            EncryptedPlayerPrefs.SetString("ShopUpdateTime", tempTime);
            if (priceChangeCallback != null) priceChangeCallback(UpdateMarketPower());
        }
        hour++;
        hour %= 24;
    }

    private float UpdateMarketPower()    // 시장 배율
    {
        // 여러 아이템이 다르게 가격이 설정되도록 update마다 랜덤하게 variance 설정  
        float variance = UnityEngine.Random.Range(0.1f, 0.2f);
        currentPosition += variance;

        float perlinNoise = Mathf.PerlinNoise(currentPosition, 0);

        // 0~1사이 perlinNoise 값을 +-30%로 선형 보간 처리함.
        float perlinNoiseLerp = Mathf.Lerp(0.7f, 1.3f, perlinNoise);
        float afterMarketPower = marketPower * perlinNoiseLerp;

        return afterMarketPower;
    }
}
