using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public int id;
    public ItemInfo info;
    public int amount;
    public string obtainDate;
    public int prevPrice;
    public PriceStatus priceStatus = PriceStatus.keep;

    public ItemData(ItemInfo info)
    {
        this.id = info.code;
        this.info = info;
        this.amount = 0;
        this.prevPrice = info.sellCost;
    }
}

public enum PriceStatus
{
    raise,
    fall,
    keep
}
