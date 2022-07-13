 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class FishSaveManager
{
    public static void Save(FishGrowTime[] fishs)
    { 
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "Fish Save.bin");
        
        FileStream stream =File.Create(path);

        FishSaveData[] fishSaveDatas = new FishSaveData[fishs.Length];
        for(int i = 0; i<fishs.Length; ++i){
            fishSaveDatas[i] = new FishSaveData(fishs[i]);
        }

        AllFishsData data = new AllFishsData(fishSaveDatas);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static AllFishsData Load()
    {
        try{
            string path = Path.Combine(Application.persistentDataPath, "Fish Save.bin");
            if (File.Exists(path))
            {
                FileStream stream = File.OpenRead(path);
                BinaryFormatter formatter = new BinaryFormatter();
                AllFishsData data = (AllFishsData)formatter.Deserialize(stream);
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
