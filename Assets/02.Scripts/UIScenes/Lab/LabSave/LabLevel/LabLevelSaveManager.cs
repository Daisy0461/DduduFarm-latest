 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class LabLevelSaveManager
{
    public static void Save(UpgradeLevel[] up){ 
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.dataPath, "LabLevel Save.bin");
        
        FileStream stream =File.Create(path);

        LabLevelSaveData[] levelSaves = new LabLevelSaveData[up.Length];

        for(int i = 0; i<up.Length; ++i){
            levelSaves[i] = new LabLevelSaveData(up[i]);
        }

        AllLabLevel data = new AllLabLevel(levelSaves);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static AllLabLevel Load(){
        try{
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Path.Combine(Application.dataPath, "LabLevel Save.bin");
            FileStream stream = File.OpenRead(path);
            AllLabLevel data = (AllLabLevel)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        catch(Exception e){
            Debug.Log(e.Message);
            return default;
        }
     }

}
