 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class CropSaveManager
{
    public static void Save(CropGrowTime[] crops)
    { 
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "Crop Save.bin");
        
        FileStream stream =File.Create(path);

        CropSaveData[] cropSaveDatas = new CropSaveData[crops.Length];
        for(int i = 0; i<crops.Length; ++i){
            cropSaveDatas[i] = new CropSaveData(crops[i]);
        }

        AllCropsData data = new AllCropsData(cropSaveDatas);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static AllCropsData Load()
    {
        try {
            string path = Path.Combine(Application.persistentDataPath, "Crop Save.bin");
            if (File.Exists(path))
            {
                FileStream stream = File.OpenRead(path);
                BinaryFormatter formatter = new BinaryFormatter();
                AllCropsData data = (AllCropsData)formatter.Deserialize(stream);
                stream.Close();
                return data;
            }
            else 
            {
                return default;
            }
        } catch(Exception e) {
            Debug.Log(e.Message);
            return default;
        }
     }

}
