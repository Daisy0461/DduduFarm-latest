using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : DataManager<BuildingManager, BuildingInfo, BuildingData>
{
    public override void AddInfo(BuildingInfo info)
    {
        if (infoDict.ContainsKey(info.code)) return;
        infoDict.Add(info.code, info);
    }

    public Dictionary<int, BuildingInfo>.KeyCollection InfoKeys()
    {
        return infoDict.Keys;
    } 

    public override void AddData(int code, int amount=1)
    {
        BuildingData data = new BuildingData(GetInfo(code));
        dataList.Add(data);  
        Save();
    } 

    public override BuildingData GetData(int id)
    {
        foreach (var data in dataList)
        {
            if (data.id == id)
                return data;
        }
        return null;
    }

    public BuildingData GetUnBuildedData(int code)
    {
        foreach (var data in dataList)
        {
            if (data.info.code == code)
            {
                if (data.isBuilded == false)
                {
                    return data;
                }
            }
        }
        return null;
    }

    public int GetBuildingAmount(int code)
    {
        int amount = 0;
        foreach (var data in dataList)
        {
            if (data.info.code == code)
                amount++;
        }
        return (amount);
    }

    public int GetBuildedAmount(int code)
    {
        int amount = 0;
        foreach (var data in dataList)
        {
            if (data.info.code == code)
                amount += (data.isBuilded == true) ? 1 : 0;
        }
        return (amount);
    }

    public int GetBuildingAmount(bool isBuilded)
    {
        int amount = 0;
        foreach (var data in dataList)
        {
            if (data.isBuilded == isBuilded)
                amount++;
        }
        return amount;
    }

    public int GetUniqueUnBuildedAmount()
    {
        int amount = 0;
        bool[] allBuildings = new bool[100];
        foreach (var data in dataList)
        {
            if (data.isBuilded == false && !allBuildings[data.info.code])
            {
                allBuildings[data.info.code] = true;
                amount++;
            }
        }
        return amount;
    }

    public List<BuildingData> GetUniqueUnBuildedBuilding()
    {
        List<BuildingData> datas = new List<BuildingData>();
        bool[] allBuildings = new bool[100];
        foreach(var data in dataList)
        {
            if (data.isBuilded == false && !allBuildings[data.info.code])
            {
                allBuildings[data.info.code] = true;
                datas.Add(data);
            }
        }
        return datas;
    }

    public void RemoveSomeData(int code, int amount=1)    // code 가 같은 것을 지우기
    {
        for (int i=dataList.Count-1; i>=0; i--)
        {
            if (dataList[i].info.code == code)
            {
                int index = dataList[i].id;
                RemoveData(GetData(dataList[i].id));
                amount--;
                if (amount <= 0) break;
            }
        }
        if (amount > 0)
            Debug.LogError("no Building Data to Remove");
        Save();
    }

    public void RemoveData(BuildingData data)
    {
        if (dataList.Contains(data))
            dataList.Remove(data);
        else
            Debug.LogError("no Building Data to Remove");
        Save();
    } 

    public override BuildingInfo ConvertXmlToInfo(System.Xml.XmlNode node)
    {
        var attrib = node.Attributes;
        int id = RtrnNodeToInt(attrib.GetNamedItem("id"));
        string name = RtrnNodeToStr(attrib.GetNamedItem("name"));
        string note = RtrnNodeToStr(attrib.GetNamedItem("note"));
        string imgPath = RtrnNodeToStr(attrib.GetNamedItem("imgPath"));
        int requireFull = RtrnNodeToInt(attrib.GetNamedItem("requireFull"));
        int buyCost = RtrnNodeToInt(attrib.GetNamedItem("buyCost"));
        int sellCost = RtrnNodeToInt(attrib.GetNamedItem("sellCost"));
        int matId = RtrnNodeToInt(attrib.GetNamedItem("matId"));
        int matAmount = RtrnNodeToInt(attrib.GetNamedItem("matAmount"));
        int outputId = RtrnNodeToInt(attrib.GetNamedItem("outputId"));
        int outputAmount = RtrnNodeToInt(attrib.GetNamedItem("outputAmount"));
        int cycleTime = RtrnNodeToInt(attrib.GetNamedItem("cycleTime"));

        note = note.Replace("(주인공)", UserInfo.GetUserName());
        BuildingInfo info = new BuildingInfo(id, name, note, imgPath, requireFull, buyCost, sellCost, 
                        matId, matAmount, outputId, outputAmount, cycleTime);
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