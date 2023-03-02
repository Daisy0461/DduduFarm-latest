using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DduduManager : DataManager<DduduManager, DduduInfo, DduduData>
{
    private int _maxDduduCount = 10;
    public int MaxDduduCount => _maxDduduCount;

    public void SetMaxDduduCount(int value)
    {
        _maxDduduCount = value;
    }

    public int GetWorkDduduCount()
    {
        int count = 0;
        foreach (var ddudu in dataList)
        {
            if (ddudu.isWork)
            {
                count++;
            }

        }
        return count;
    }

	public override void AddInfo(DduduInfo info)
    {
        if (infoDict.ContainsKey(info.code)) return;
        infoDict.Add(info.code, info);
    }

    public override DduduData GetData(int id)
    {
        foreach (var data in dataList)
        {
            if (data.id == id)
                return data;
        }
        return null;
    }

    public override void SetData(int id, DduduData data)
    {
        if (IsDataExist(id)) 
        {
            int index = dataList.IndexOf(GetData(id));
            dataList[index] = data;
        } 
        else 
            Debug.LogError("There is no id in the dataList");
    }

    public int AddData(int code)
    {
        DduduData data = new DduduData(GetInfo(code));
        while(IsDataExist(data.id))
        {
            data.id += 1;
            if (data.id < 0)
                data.id *= -1;
        }
        dataList.Add(data);
        Save();
        return data.id;
    }

    public void RemoveData(int id, int amount=1)
    {
        if (IsDataExist(id))
        {   
            DduduData data = GetData(id);
            dataList.Remove(data);
            Save();
        }
    }

	public override DduduInfo ConvertXmlToInfo(System.Xml.XmlNode node)
    {
        var attrib = node.Attributes;
        int id = RtrnNodeToInt(attrib.GetNamedItem("id"));
        string name = RtrnNodeToStr(attrib.GetNamedItem("name"));
        string note = RtrnNodeToStr(attrib.GetNamedItem("note"));
        string imgPath = RtrnNodeToStr(attrib.GetNamedItem("imgPath"));
        int maxFull = RtrnNodeToInt(attrib.GetNamedItem("maxFull"));
        int maxLike = RtrnNodeToInt(attrib.GetNamedItem("maxLike"));
        int gem1Id = RtrnNodeToInt(attrib.GetNamedItem("gem1Id"));
        int gem2Id = RtrnNodeToInt(attrib.GetNamedItem("gem2Id"));

        DduduInfo info = new DduduInfo(id, name, note, imgPath, maxFull, maxLike, gem1Id, gem2Id);
        return info;
    }

	public override void Save()
	{
        if (SceneManager.GetActiveScene().name == "Farm")
        {
            var ddudus = GameObject.FindObjectsOfType<Ddudu>();
            foreach (var ddudu in ddudus)
                SetData(ddudu.data.id, ddudu.data.DduduDataSave(ddudu));
        }
		base.Save();
	}

    private void OnApplicationFocus(bool focusStatus) 
    {
        if (focusStatus == true)
        {
            Load();
        }    
    }

	public override void Load()
	{
		base.Load();
	}
}
