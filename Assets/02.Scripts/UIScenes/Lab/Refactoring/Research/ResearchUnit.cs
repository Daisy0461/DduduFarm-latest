using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchUnit : MonoBehaviour
{
    [System.Serializable]
    public struct RequireMaterial
    {
        public int requireMaterialId;
        public int requireMaterialCount;

        public RequireMaterial(int requireMaterialId, int requireMaterialCount)
        {
            this.requireMaterialId = requireMaterialId;
            this.requireMaterialCount = requireMaterialCount;
        }
    }

    [HideInInspector]
    public List<RequireMaterial> requireMaterialList;

    public int researchID;

    public ResearchInfo researchInfo;

    public void SetResearchUnitFromInfoDict()
    {
        researchInfo = ResearchManager.Instance.GetInfo(researchID);

        foreach (var requireMaterial in researchInfo.requireMaterial)
        {
            if (requireMaterial.Key != 0)
            {
                requireMaterialList.Add(new RequireMaterial(requireMaterial.Key, requireMaterial.Value));
            }
        }
    }
}
