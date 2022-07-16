using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class XMLManager : SingletonBase<XMLManager>
{
    public Dictionary<int, ItemInfo> LoadItemManagerItemInfoList()
    {
        ItemInfo itemInfo;
        Dictionary<int, ItemInfo> itemInfoList = new Dictionary<int, ItemInfo>();

        TextAsset textAsset = (TextAsset)Resources.Load("XML/ItemInfoList");

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);

        XmlNodeList nodes = xmlDoc.SelectNodes("ItemInfoList/ItemInfo");

        if (nodes != null)
        {
            foreach (XmlNode node in nodes)
            {
                itemInfo = new ItemInfo(
                    Int32.Parse(node.SelectSingleNode("ID").InnerText.ToString()),
                    node.SelectSingleNode("NAME").InnerText,
                    node.SelectSingleNode("TYPE").InnerText,
                    node.SelectSingleNode("IMGPATH").InnerText,
                    Int32.Parse(node.SelectSingleNode("BUYCOST").InnerText),
                    Int32.Parse(node.SelectSingleNode("SELLCOST").InnerText));

                itemInfoList.Add(Int32.Parse(node.SelectSingleNode("ID").InnerText), itemInfo);
            }
        }

        return itemInfoList;
    }
}
