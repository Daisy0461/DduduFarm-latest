using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DduduManager : DataManager<DduduManager, DduduInfo, DduduData>
{
    private int _maxDduduCount = 10;
    public int MaxDduduCount => _maxDduduCount;
    public List<Ddudu> DduduObjects { get; private set; } = new List<Ddudu>();

    public Ddudu InstantiateDdudu(int code)
    {
        Instantiate(Resources.Load<GameObject>($"{PathAlias.ddudu_prefab_path}{code}"))
        .TryGetComponent<Ddudu>(out var ddudu);
        if (ddudu == null) Debug.Log("null 이랍니다");
        DduduObjects.Add(ddudu);
        return ddudu;
    }

    public static Ddudu SpawnDdudu(int id, Vector3 pos, bool isNew = false)
    {
        if (isNew)
        {
            id = DduduManager.Instance.AddData(id);
        }
        var data = DduduManager.Instance.GetData(id);
        var index = data.info.code;
        var newDdudu = DduduManager.Instance.InstantiateDdudu(index);
        newDdudu.transform.SetPositionAndRotation(pos, Quaternion.Euler(0f, 0f, 0f));

        if (data.interest == -1)
        {
            int ran = Random.Range((int)DataTable.Craft+1, (int)DataTable.Craft + 10);
            data.interest = ran;
        }
        newDdudu.data = data;
        
        Profile.dduduList.Add(index);
        return newDdudu;
    }

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

    public bool RemoveData(int id, int amount=1)
    {
        if (!IsDataExist(id)) return false;
        DduduData data = GetData(id);
        dataList.Remove(data);
        for (int index = 0; index < DduduObjects.Count; index++)
        {
            if (DduduObjects[index].data.id == id)
            {
                DduduObjects.Remove(DduduObjects[index]);
                DduduObjects[index].gameObject.SetActive(false);
                break;
            }
        }
        Save();
        return true;
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
