using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : DataManager<FishManager, FishInfo, ItemData>
{
	public override void AddInfo(FishInfo info)
    {
        if (infoDict.ContainsKey(info.code)) return;
        infoDict.Add(info.code, info);
    }
    
    public override FishInfo ConvertXmlToInfo(System.Xml.XmlNode node)
    {
        var attrib = node.Attributes;
        int code = RtrnNodeToInt(attrib.GetNamedItem("id"));
        string name = RtrnNodeToStr(attrib.GetNamedItem("name"));
        string note = RtrnNodeToStr(attrib.GetNamedItem("note"));
        string imgPath1 = RtrnNodeToStr(attrib.GetNamedItem("imgPath1"));
        string imgPath2 = RtrnNodeToStr(attrib.GetNamedItem("imgPath2"));
        int grow1Time = RtrnNodeToInt(attrib.GetNamedItem("grow1Time"));
        int grow2Time = RtrnNodeToInt(attrib.GetNamedItem("grow2Time"));
        int full = RtrnNodeToInt(attrib.GetNamedItem("full"));
        int like = RtrnNodeToInt(attrib.GetNamedItem("like"));

        FishInfo info = new FishInfo(code, name, note, imgPath1, imgPath2, 
                    grow1Time, grow2Time, full, like);
        return info;
    }
}
