using System.Collections.Generic;
using UnityEngine;
using System;

public class ResearchManager : DataManager<ResearchManager, ResearchInfo, ResearchData>
{
    const int NO_PRE_RESEARCH = (int)DataTable.Common;

	public override void AddInfo(ResearchInfo info)
    {
        if (infoDict.ContainsKey(info.code)) return;
        infoDict.Add(info.code, info);
    }

    public override void AddData(int id, int amount=1)
    {
        try
        {
            ResearchData data = new ResearchData(infoDict[id]);
            int preId = data.info.preCode;

            if (preId != NO_PRE_RESEARCH)
            {
                if (dataList[preId].info.name == data.info.name)
                {
                    dataList[preId].isPrime = false;
                }
            }

            data.isPrime = true;

            dataList.Add(data);
            Save();
        } 
        catch(Exception e)
        {
            Debug.LogError(e.Message);
            return;
        }
    }

    public override ResearchData GetData(int id)
    {
        foreach(var data in dataList)
        {
            if (data.id == id)
                return data;
        }
        return null;
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
        int level = RtrnNodeToInt(attrib.GetNamedItem("researchLv"));
        string researchValue = RtrnNodeToStr(attrib.GetNamedItem("researchVal"));
        Dictionary<int, int> requireMaterial = new Dictionary<int, int>();

        for (int i=1; i<4; ++i)
        {
            int matId = RtrnNodeToInt(attrib.GetNamedItem("mat" + i.ToString() + "Id"));
            int matAmount = RtrnNodeToInt(attrib.GetNamedItem("mat" + i.ToString() + "Amount"));

            if (matId != 0)
            {
                requireMaterial.Add(matId, matAmount);
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
