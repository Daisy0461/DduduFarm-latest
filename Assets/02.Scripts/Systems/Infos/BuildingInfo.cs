[System.Serializable]
public class BuildingInfo
{
    public int code;
    public string name;
    public string note;
    public string imgPath;
    public int requireFull;
    public int buyCost;
    public int sellCost;
    public int matId;
    public int matAmount;
    public int outputId;
    public int outputAmount;
    public int cycleTime;

    public BuildingInfo(int id, string name, string note, string imgPath, int requireFull, int buyCost, int sellCost, 
                        int matId, int matAmount, int outputId, int outputAmount, int cycleTime)
    {
        this.code = id;
        this.name = name;
        this.note = note;
        this.imgPath = imgPath;
        this.requireFull = requireFull;
        this.buyCost = buyCost;
        this.sellCost = sellCost;
        this.matId = matId;
        this.matAmount = matAmount;
        this.outputId = outputId;
        this.outputAmount = outputAmount;
        this.cycleTime = cycleTime;
    }
}

