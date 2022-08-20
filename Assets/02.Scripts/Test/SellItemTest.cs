using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItemTest : MonoBehaviour
{
    [SerializeField] ShopUpdateTest shopUpdateTest;

    void Start()
    {
        shopUpdateTest.priceChangeCallback += this.PriceChange;   
    }

    void Update()
    {
        
    }

    public void PriceChange()
    {

    }
}
