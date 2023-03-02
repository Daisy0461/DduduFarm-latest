using UnityEngine;
using System.Xml;

public class XMLParser : MonoBehaviour
{
#region files
    string itemFile = "Data/ItemFile";
    string buildingFile = "Data/BuildingFile";
    string researchFile = "Data/ResearchFile";
    string dduduFile = "Data/DduduFile";
    string cropFile = "Data/CropFile";
    string fishFile = "Data/FishFile";
#endregion
    
    void Awake()
    {
        LoadParseXML<ItemManager, ItemInfo, ItemData>(itemFile);
        LoadParseXML<BuildingManager, BuildingInfo, BuildingData>(buildingFile);
        LoadParseXML<ResearchManager, ResearchInfo, ResearchData>(researchFile);
        LoadParseXML<DduduManager, DduduInfo, DduduData>(dduduFile);
        LoadParseXML<CropManager, CropInfo, CropInfo>(cropFile);
        LoadParseXML<FishManager, FishInfo, ItemData>(fishFile);
    }

    public static void LoadParseXML<T, Info, Data>(string fileName)
        where T : DataManager<T, Info, Data>
    {
        T manager = DataManager<T, Info, Data>.Instance;

        TextAsset txtAsset = (TextAsset)Resources.Load(fileName);
        XmlDocument xmlDoc = new XmlDocument();
        // Debug.Log(txtAsset.text); // 파일 내용 확인 로그. 모든 파일의 테스트가 끝나면 제거
        xmlDoc.LoadXml(txtAsset.text);
 
        XmlNodeList all_nodes = xmlDoc.SelectNodes("Items");
        foreach (XmlNode node in all_nodes)
        {   
            if (node.Name.Equals("Items") && node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    Info info = manager.ConvertXmlToInfo(child);
                    manager.AddInfo(info);
                }
            }            
        }
    }
}