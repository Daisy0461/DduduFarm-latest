using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemManager : DataManager<ItemManager, ItemInfo, ItemData>
{
    public int maxDataListValue = 15;

	public override void AddInfo(ItemInfo info)
    {
        if (infoDict.ContainsKey(info.code)) return;
        infoDict.Add(info.code, info);
    }

    public Dictionary<int, ItemInfo> GetInfoDict()
    {
        return infoDict;
    }

    public override int GetDataListCount()
    {
        int ret = dataList.Count;
        if (IsDataExist((int)DataTable.Love))
            ret--;
        if (IsDataExist((int)DataTable.Money))
            ret--;
        return ret;
    }

    public int GetDataListCount(bool isShop)
    {
        int ret = dataList.Count;
        if (isShop)
        {
            foreach (var data in dataList)
            {
                if (data.info.sellCost <= 0)
                    ret--;
            }
        }
        return ret;
    }

    public new bool AddData(int id, int amount=1)
    {
        bool ret = true;
        string errMessage = "";
        try {
            ItemData data = GetData(id);
            if (data != null)
            {
                if ((data.amount + amount) < 0) throw new OverflowException();
                data.amount += amount;
                data.obtainDate = DateTime.Now.ToLocalTime().ToBinary().ToString();    
            }
            else
            {
                if (GetDataListCount() == maxDataListValue) throw new ArgumentOutOfRangeException();
                data = new ItemData(GetInfo(id));
                if ((data.amount + amount) < 0) throw new OverflowException();
                data.amount += amount;
                data.obtainDate = DateTime.Now.ToLocalTime().ToBinary().ToString();
                dataList.Add(data);
            }
            Save();
        } catch(OverflowException) {
            errMessage = "해당 아이템을 더 이상 가질 수 없습니다.";
            ret = false;
        } catch(ArgumentOutOfRangeException) {
            errMessage = "가질 수 있는 아이템 수를 초과했습니다.";
            ret = false;
        } catch(Exception e) {
            errMessage = e.Message;
            ret = false;
        } finally {
            if (ret == false)
            {
                var Err = GameObject.FindGameObjectWithTag("Error")?.transform;
                if (Err != null)
                {
                    Err.GetChild(0).gameObject.SetActive(true); // quit
                    Err.GetChild(1).GetComponent<TextObject>().contentText.text = errMessage;
                    Err.GetChild(1).gameObject.SetActive(true); // popup
                }
            }  
        }
        return ret;
    }

    public override ItemData GetData(int id)
    {
        foreach (var data in dataList)
        {
            if (data.id == id)
                return data;
        }
        return null;
    }

    public void ResetDataList()
    {
        dataList.Clear();
        Save();
    }
    
    public void RemoveData(int id, int amount=1)
    {
        if (IsDataExist(id))
        {   
            ItemData data = GetData(id);
            data.amount -= amount;
            if (data.amount <= 0)
                dataList.Remove(data);
            Save();
        }
    }

    public override ItemInfo ConvertXmlToInfo(System.Xml.XmlNode node)
    {
        var attrib = node.Attributes;
        int id = RtrnNodeToInt(attrib.GetNamedItem("id"));
        string name = RtrnNodeToStr(attrib.GetNamedItem("name"));
        string note = RtrnNodeToStr(attrib.GetNamedItem("note"));
        string imgPath = RtrnNodeToStr(attrib.GetNamedItem("imgPath"));
        int buyCost = RtrnNodeToInt(attrib.GetNamedItem("buyCost"));
        int sellCost = RtrnNodeToInt(attrib.GetNamedItem("sellCost"));
        
        ItemInfo info = new ItemInfo(id, name, note, imgPath, buyCost, sellCost);
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
        EncryptedPlayerPrefs.SetInt("maxDataListValue", maxDataListValue);
    }

	public override void Load()
	{
		base.Load();
        maxDataListValue = EncryptedPlayerPrefs.GetInt("maxDataListValue" , 15);    
	}
}