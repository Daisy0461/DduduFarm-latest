using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DduduRecipeManager : SingletonBase<DduduRecipeManager>
{
    private Dictionary<int, DduduRecipeInfo> infos = new Dictionary<int, DduduRecipeInfo>();

    public void AddInfo(DduduRecipeInfo info)
    {
        if (infos.ContainsKey(info.resultId)) return;
        infos.Add(info.resultId, info);
    }

    public DduduRecipeInfo GetInfo(int resultId)
    {
        return infos[resultId];
    }

    public bool TryGetResult(int mat1Id, int mat2Id, out int resultId)
    {
        resultId = 0;
        foreach (var info in infos.Values)
        {
            if (!((info.mat1Id == mat1Id && info.mat2Id == mat2Id) ||
                (info.mat1Id == mat2Id && info.mat2Id == mat1Id))) continue;
            resultId = info.resultId;
            return true;
        }
        return false;
    }  
}

public class DduduRecipeInfo
{
    public int resultId;
    public string resultName;
    public int mat1Id;
    public int mat2Id;
    public int needCount;   
}
