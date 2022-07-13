using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DduduUnit : MonoBehaviour
{
    [System.Serializable]
    public struct RequireMaterial
    {
        public int requireMaterialId;
        public int requireMaterialCount;
    }

    // type은 그냥 dduduID 쓰면 되지 않나..?
    public int dduduID;
    public int requireResearchID;
    public string callDduduImagePath;
    public string dduduName;
    public string dduduExplain;

    [SerializeField]
    public List<RequireMaterial> requireMaterialList;
}
