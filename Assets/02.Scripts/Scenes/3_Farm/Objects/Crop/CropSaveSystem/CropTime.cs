using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CropTime : CropSaveLoad      
{
    private DateTime m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();

    void Awake()
    {
        LoadAppQuitTime();  
    }
    
    public void OnApplicationFocus(bool value)
    {
        if (value)
        {  
            LoadCrop();     
            LoadAppQuitTime();         
        }
        else
        {
            SaveCrop();
            SaveAppQuitTime();
        }
    }

    public void OnApplicationQuit()
    {
        SaveCrop();
        SaveAppQuitTime();
    }

    public bool SaveAppQuitTime()           //나간 시간 저장
    {
        bool result = false;
        try
        {
            var appQuitTime = DateTime.Now.ToLocalTime().ToBinary().ToString();     
            PlayerPrefs.SetString("AppQuitTime", appQuitTime);
            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }

    public bool LoadAppQuitTime()       
    {
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("AppQuitTime"))
            {
                var appQuitTime = string.Empty;
                appQuitTime = PlayerPrefs.GetString("AppQuitTime");                
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime));
            }

            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }

    public DateTime get_m_AppQuitTime(){
        return m_AppQuitTime;
    }
}
