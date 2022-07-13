public class CropInfo
{
    public int code;
    public string name;
    public string note;
    public string imgPath1;
    public string imgPath2;
    public string imgPath3;
    public int grow1Time;
    public int grow2Time;
    public int havestMin;
    public int havestMax;

    public CropInfo(int code, string name, string note, string imgPath1, string imgPath2, string imgPath3, 
                    int grow1Time, int grow2Time, int havestMin, int havestMax)
    {
        this.code = code;
        this.name = name;
        this.note = note;
        this.imgPath1 = imgPath1;
        this.imgPath2 = imgPath2;
        this.imgPath3 = imgPath3;
        this.grow1Time = grow1Time;
        this.grow2Time = grow2Time;
        this.havestMin = havestMin;
        this.havestMax = havestMax;
    }
}


