using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingAttrib : MonoBehaviour
{
    public BuildingData data;
    protected DateTime m_AppQuitTime;

    private void OnApplicationFocus(bool focusStatus) 
    {
        // data = BuildingManager.Instance.GetData(buildingId);
    }

    public bool LoadAppQuitTime() 
    { 
        bool result = false; 
        try 
        { 
            if (PlayerPrefs.HasKey("AppQuitTime")) 
            { 
                var appQuitTime = PlayerPrefs.GetString("AppQuitTime"); 
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime)); 
            }
            else 
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(DateTime.Now.ToLocalTime().ToBinary().ToString())); 
            result = true; 
        } 
        catch (System.Exception e) 
        {
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")"); 
        } 
        return result; 
    }
}
