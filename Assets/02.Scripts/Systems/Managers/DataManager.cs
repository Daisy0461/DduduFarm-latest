using System.Collections.Generic;
using UnityEngine;

public abstract class DataManager<T, Info, Data> : SingletonBase<T>
    where T : MonoBehaviour
{
    [SerializeField] protected Dictionary<int, Info> infoDict = new Dictionary<int, Info>();
    [SerializeField] protected List<Data> dataList = new List<Data>();

    public abstract void AddInfo(Info info);

    public Info GetInfo(int code)
    {
        if (infoDict.ContainsKey(code))
            return infoDict[code];
        return default(Info);
    }

    public virtual bool IsDataExist(int id)
    {
        if (GetData(id) != null)
            return true;
        return false;
    }

    public virtual void AddData(int id, int amount=1){}
    public virtual Data GetData(int id)
    {
        return default;
    }
    public virtual void SetData(int id, Data data){}
    
    public virtual List<Data> GetDataList()
    {
        return dataList;
    }

    public virtual int GetDataListCount()
    {
        return dataList.Count;
    }

    public abstract Info ConvertXmlToInfo(System.Xml.XmlNode node);
    
    public string RtrnNodeToStr(System.Xml.XmlNode node)
    {
        if (node == null) return "";
        return node.Value;
    }

    public int RtrnNodeToInt(System.Xml.XmlNode node)
    {
        string str = RtrnNodeToStr(node);
        if (str == null || str == "") return 0;
        return int.Parse(str);
    }
    
    public virtual void Save()
    {
        string fileName = Instance.GetType().ToString();
        fileName = fileName.Split('M')[0] + "SaveData.bin";
        SaveManager.Save<Data>(dataList, fileName);
    }

    public virtual void Load()
    {
        string fileName = Instance.GetType().ToString();
        fileName = fileName.Split('M')[0] + "SaveData.bin";
        dataList = SaveManager.Load<Data>(fileName);
    }
}
