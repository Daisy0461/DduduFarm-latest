using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResearchData
{
    public int Id;
    public bool IsResearched;

    public ResearchData(ResearchInfo info)
    {
        this.Id = info.code;
        this.IsResearched = false;
    }
}
