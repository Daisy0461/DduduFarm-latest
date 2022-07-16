using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : DataManager<CropManager, CropInfo, CropInfo>
{
	public override void AddInfo(CropInfo info)
    {
        if (infoDict.ContainsKey(info.code)) return;
        infoDict.Add(info.code, info);
    }

    public override CropInfo ConvertXmlToInfo(System.Xml.XmlNode node)
    {
        var attrib = node.Attributes;
        int code = RtrnNodeToInt(attrib.GetNamedItem("id"));
        string name = RtrnNodeToStr(attrib.GetNamedItem("name"));
        string note = RtrnNodeToStr(attrib.GetNamedItem("note"));
        string imgPath1 = RtrnNodeToStr(attrib.GetNamedItem("imgPath1"));
        string imgPath2 = RtrnNodeToStr(attrib.GetNamedItem("imgPath2"));
        string imgPath3 = RtrnNodeToStr(attrib.GetNamedItem("imgPath3"));
        int grow1Time = RtrnNodeToInt(attrib.GetNamedItem("grow1Time"));
        int grow2Time = RtrnNodeToInt(attrib.GetNamedItem("grow2Time"));
        int havestMin = RtrnNodeToInt(attrib.GetNamedItem("havestMin"));
        int havestMax = RtrnNodeToInt(attrib.GetNamedItem("havestMax"));
        
        CropInfo info = new CropInfo(code, name, note, imgPath1, imgPath2, imgPath3, 
                    grow1Time, grow2Time, havestMin, havestMax);
        return info;
    }
}
