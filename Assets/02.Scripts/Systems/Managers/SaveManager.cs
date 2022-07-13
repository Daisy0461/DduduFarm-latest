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
}
