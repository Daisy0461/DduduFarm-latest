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
    string recipeFile = "Data/DduduRecipe";
#endregion
    
    void Awake()
    {
        LoadParseXML<ItemManager, ItemInfo, ItemData>(itemFile);
        LoadParseXML<BuildingManager, BuildingInfo, BuildingData>(buildingFile);
        LoadParseXML<ResearchManager, ResearchInfo, ResearchData>(researchFile);
        LoadParseXML<DduduManager, DduduInfo, DduduData>(dduduFile);
        LoadParseXML<CropManager, CropInfo, CropInfo>(cropFile);
        LoadParseXML<FishManager, FishInfo, ItemData>(fishFile);
        LoadParseXMLForRecipe(recipeFile);
    }

    public static void LoadParseXML<T, Info, Data>(string fileName)
        where T : DataManager<T, Info, Data>
    {
        T manager = DataManager<T, Info, Data>.Instance;

        TextAsset txtAsset = (TextAsset)Resources.Load(fileName);
        XmlDocument xmlDoc = new XmlDocument();
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

    public static void LoadParseXMLForRecipe(string fileName)
    {
        DduduRecipeManager manager = DduduRecipeManager.Instance;
        
        TextAsset txtAsset = (TextAsset)Resources.Load(fileName);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(txtAsset.text);
        XmlNodeList all_nodes = xmlDoc.SelectNodes("Items");
        foreach (XmlNode node in all_nodes)
        {
            if (node.Name.Equals("Items") && node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    var info = new DduduRecipeInfo()
                    {
                        resultId = int.Parse(child.Attributes.GetNamedItem("resultId").Value),
                        resultName = child.Attributes.GetNamedItem("resultName").Value,
                        mat1Id = int.Parse(child.Attributes.GetNamedItem("mat1Id").Value),
                        mat2Id = int.Parse(child.Attributes.GetNamedItem("mat2Id").Value),
                        needCount = int.Parse(child.Attributes.GetNamedItem("needCount").Value)
                    };
                    manager.AddInfo(info);
                }
            }
        }

    }
}