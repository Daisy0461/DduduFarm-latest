using System.Collections.Generic;
using UnityEngine;
using System;

public class ResearchManager : DataManager<ResearchManager, ResearchInfo, ResearchData>
{
    public enum ResearchType   
    {
        Farm,
        Fish,
        Structure,
        Inventory
    }    

	public override void AddInfo(ResearchInfo info)
    {
        if (infoDict.ContainsKey(info.code)) return;
        infoDict.Add(info.code, info);
    }

    public override void AddData(int id, int amount=1)
    {
        var info = GetInfo(id);
        if (info == null) 
        {
            Debug.LogError($"research info is null {id}");
            return;
        }

        var data = new ResearchData(info);
        dataList.Add(data);
    }

    public bool TryGetInfo(int code, out ResearchInfo info)
    {
        info = GetInfo(code);
        if (info != null) return true;
        return false;
    }

    public override ResearchData GetData(int id)
    {
        foreach(var data in dataList)
        {
            if (data.Id == id)
                return data;
        }
        return null;
    }

    public ResearchData GetDataExactly(int id)
    {
        if (!IsDataExist(id))
        {
            AddData(id);
        }
        return GetData(id);
    }

    public List<ResearchInfo> GetResearchInfosByResearchType(ResearchType type)
    {
        var (startId, endId) = (0,0);
        switch (type)
        {
            case ResearchType.Farm:
            {
                (startId, endId) = (1211, 1223);
                break;
            }
            case ResearchType.Fish:
            {
                (startId, endId) = (1401, 1423);
                break;
            }
            case ResearchType.Structure:
            {
                (startId, endId) = (1001, 1060);
                break;
            }
            case ResearchType.Inventory:
            {
                (startId, endId) = (1502, 1505);
                break;
            }
        }
        var researchInfos = new List<ResearchInfo>();
        for (int id = startId; id <= endId; id++)
        {
            if (!TryGetInfo(id, out var info)) continue;
            researchInfos.Add(info);    
        }
        return researchInfos;
    }

    public override void SetData(int id, ResearchData data)
    {
        base.SetData(id, data);
        for (int index = 0; index < dataList.Count; index++)
        {
            if (dataList[index].Id == data.Id)
            {
                dataList[index] = data;
                break;
            }
        }
    }

    public void SetDataListClear()
    {
        dataList = new List<ResearchData>();
    }

    public override ResearchInfo ConvertXmlToInfo(System.Xml.XmlNode node)
    {
        var attrib = node.Attributes;
        int id = RtrnNodeToInt(attrib.GetNamedItem("id"));
        int preId = RtrnNodeToInt(attrib.GetNamedItem("preId"));
        string note = RtrnNodeToStr(attrib.GetNamedItem("note"));
        string imgPath = RtrnNodeToStr(attrib.GetNamedItem("imgPath"));
        string name = RtrnNodeToStr(attrib.GetNamedItem("researchName"));
        int level = RtrnNodeToInt(attrib.GetNamedItem("researchLV"));
        string researchValue = RtrnNodeToStr(attrib.GetNamedItem("researchVal"));
        List<(int, int)> requireMaterial = new List<(int, int)>();

        for (int i=1; i<4; ++i)
        {
            int matId = RtrnNodeToInt(attrib.GetNamedItem("mat" + i.ToString() + "Id"));
            int matAmount = RtrnNodeToInt(attrib.GetNamedItem("mat" + i.ToString() + "Amount"));

            if (matId != 0)
            {
                requireMaterial.Add((matId, matAmount));
            }
        }

        ResearchInfo info = new ResearchInfo(id, preId, note, imgPath, name, level, researchValue, requireMaterial);
        return info;
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus == true) Load();
        else Save();
    }

    public override void Save()
    {
        base.Save();
    }

    public override void Load()
    {
        base.Load();
    }
}
