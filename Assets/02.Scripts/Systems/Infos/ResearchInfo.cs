using System;
using System.Collections.Generic;

[System.Serializable]
public class ResearchInfo
{
    public int code;
    public int preCode;
    public string note;
    public string imgPath;
    public string name;
    public int level;
    public string researchValue;
    public List<(int code, int count)> requireMaterial;

    public ResearchInfo(int id, int preId, string note, string imgPath, string name, int level, string researchValue, List<(int, int)> requireMaterial)
    {
        this.code = id;
        this.preCode = preId;
        this.note = note;
        this.imgPath = imgPath;
        this.name = name;
        this.level = level;
        this.researchValue = researchValue;
        this.requireMaterial = requireMaterial;
    }
}
