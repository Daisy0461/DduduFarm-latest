using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveManager : MonoBehaviour
{
    public static void Save<Data>(List<Data> dataList, string fileName)
	{ 
        try
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            FileStream stream = File.Create(path);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, dataList);
            stream.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static List<Data> Load<Data>(string fileName)
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path)) 
            {
                FileStream stream = File.OpenRead(path);
                BinaryFormatter formatter = new BinaryFormatter();
                List<Data> dataList = (List<Data>)formatter.Deserialize(stream);
                stream.Close();
                return dataList;
            } 
            else 
            {
                List<Data> dataList = new List<Data>();
                return dataList;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return default;
        }
    }

    public static void DeleteSaveData()
    {
        var fileNames = new string[5]{"BuildingSaveData.bin", "Crop Save.bin", "DduduSaveData.bin", "ItemSaveData.bin", "ResearchSaveData.bin"};
        try
        {
            foreach (var fileName in fileNames)
            {
                string path = Path.Combine(Application.persistentDataPath, fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static bool TrySaveAppQuitTime()           //나간 시간 저장
    {
        bool result = false;
        try
        {
            var appQuitTimeString = DateTime.Now.ToLocalTime().ToBinary().ToString();     
            PlayerPrefs.SetString("AppQuitTime", appQuitTimeString);
            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }

    public static bool TryLoadAppQuitTime(out DateTime appQuitTime)       
    {
        bool result = false;
        appQuitTime = DateTime.Now.ToLocalTime();
        try
        {
            if (PlayerPrefs.HasKey("AppQuitTime"))
            {
                var appQuitTimeString = string.Empty;
                appQuitTimeString = PlayerPrefs.GetString("AppQuitTime");                
                appQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTimeString));
            }

            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }
}
