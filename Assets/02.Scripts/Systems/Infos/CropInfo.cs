using UnityEngine;

public class CropInfo
{
    public int code;
    public string name;
    public string note;
    public string imgPath1;
    public string imgPath2;
    public string imgPath3;
    public int grow1Time => (int)(_grow1Time * PlayerPrefs.GetFloat("토질 연구", 1));
    public int grow2Time => (int)(_grow2Time * PlayerPrefs.GetFloat("토질 연구", 1));
    public int havestMin => (int)(_harvestMin * PlayerPrefs.GetFloat("품질 개량", 1));
    public int havestMax => (int)(_harvestMax * PlayerPrefs.GetFloat("품질 개량", 1));

    private int _grow1Time;
    private int _grow2Time;
    private int _harvestMin;
    private int _harvestMax;

    public CropInfo(int code, string name, string note, string imgPath1, string imgPath2, string imgPath3, 
                    int grow1Time, int grow2Time, int havestMin, int havestMax)
    {
        this.code = code;
        this.name = name;
        this.note = note;
        this.imgPath1 = imgPath1;
        this.imgPath2 = imgPath2;
        this.imgPath3 = imgPath3;
        this._grow1Time = grow1Time;
        this._grow2Time = grow2Time;
        this._harvestMin = havestMin;
        this._harvestMax = havestMax;
    }
}


