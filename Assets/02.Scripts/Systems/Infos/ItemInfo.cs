[System.Serializable]
public class ItemInfo
{
    public int code;
	public string name;
	public string note;
	public string imgPath;
	public int buyCost;
	public int sellCost;
	public int marketLimit;
	public int marketVolume;

    public ItemInfo(int id, string name, string note, string imgPath, int buyCost, int sellCost, int marketLimit, int marketVolume)
    {
		this.code = id;
		this.name = name;
		this.note = note;
		this.imgPath = imgPath;
		this.buyCost = buyCost;
		this.sellCost = sellCost;
		this.marketLimit = marketLimit;
		this.marketVolume = marketVolume;		
    }
}

