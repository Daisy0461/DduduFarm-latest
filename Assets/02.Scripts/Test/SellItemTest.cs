using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItemTest : MonoBehaviour
{
    int prevPrice = 100;
    int curPrice;
    
    float currentPosition = 0f;
    int basePrice = 100;

    void Awake()
    {
        ShopUpdateTest.priceChangeCallback += this.PriceChange;   
    }

    private void OnDestroy() 
    {
        ShopUpdateTest.priceChangeCallback -= this.PriceChange;
    }

    public void PriceChange(float marketPower)
    {
        Debug.Log("price Change");
        // 현재 가격 계산
        curPrice = UpdateItemPrice(
                        marketPower     // 1 - 1 / (max(수요 한계 값 / 현재 시장 규모, 1) * 2)
                            );          // 기본 가격 * 연구 배율 * 시장 배율 * 랜덤 배율

        Debug.Log(string.Format("{0} > {1}", prevPrice, curPrice));
        // if 상점 미판매 품목 이면
        //      이전 값(data)과 비교해서
        //          크다: 빨간 삼각형
                if (prevPrice < curPrice)
                    Debug.Log("red");
        //          같다: 작대기
                if (prevPrice == curPrice)
                    Debug.Log("same");
        //          작다: 파란 역삼각형
                if (prevPrice > curPrice)
                    Debug.Log("blue");

        // 이전 값(item.data.prevPrice)을 지금 값(curPrice)으로 갱신
        prevPrice = curPrice;   
    }

    public int UpdateItemPrice(float marketPower)   // 판매 가격 가격
    {
        // 여러 아이템이 다르게 가격이 설정되도록 update마다 랜덤하게 variance 설정  
        float variance = Random.Range(0.01f, 0.2f);
        currentPosition += variance;

        float perlinNoise = Mathf.PerlinNoise(currentPosition, 0);

        // 0~1사이 perlinNoise 값을 +-30%로 선형 보간 처리함.
        float perlinNoiseLerp = Mathf.Lerp(0.7f, 1.3f, perlinNoise);
        int afterPrice = Mathf.RoundToInt(basePrice * perlinNoiseLerp * marketPower);   // * 연구배율

        return afterPrice;
    }
}
