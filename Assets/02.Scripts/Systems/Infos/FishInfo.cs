[System.Serializable]
public class FishInfo
{
    public int code;
    public string name;
    public string note;
    public string imgPath1;
    public string imgPath2;
    public int grow1Time;
    public int grow2Time;
    public int full;
    public int like;
    public int gemCount;

    public FishInfo(int code, string name, string note, string imgPath1, string imgPath2, 
                    int grow1Time, int grow2Time, int full, int like, int gemCount)
    {
        this.code = code;
        this.name = name;
        this.note = note;
        this.imgPath1 = imgPath1;
        this.imgPath2 = imgPath2;
        this.grow1Time = grow1Time;
        this.grow2Time = grow2Time;
        this.full = full;
        this.like = like;
        this.gemCount = gemCount
    }
}
