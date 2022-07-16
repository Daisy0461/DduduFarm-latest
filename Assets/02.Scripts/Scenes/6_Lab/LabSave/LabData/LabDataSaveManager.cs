 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

/// <summary>
/// 1. Binary 형식으로 Save and Load
/// </summary>
public static class LabDataSaveManager
{
    public static void Save(LabData labdata)
    { 
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "Lab Save.bin");
        
        FileStream stream =File.Create(path);
        LabSaveData labDatas = new LabSaveData(labdata);
        AllLabData data = new AllLabData(labDatas);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static AllLabData Load()
    {
        try{
            string path = Path.Combine(Application.persistentDataPath, "Lab Save.bin");
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = File.OpenRead(path);
                AllLabData data = (AllLabData)formatter.Deserialize(stream);
                stream.Close();
                return data;
            }
            else
            {
                return default;
            }
        }
        catch(Exception e){
            Debug.Log(e.Message);
            return default;
        }
     }

}
