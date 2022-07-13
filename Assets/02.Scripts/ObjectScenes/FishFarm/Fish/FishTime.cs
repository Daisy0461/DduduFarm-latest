using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FishTime : FishSaveLoad
{
    //이 이후에는 시간 관련 변수들
    private DateTime m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();  //이건 나가는 시간이고 꼭 필요함!
    
    void Awake(){
        LoadAppQuitTime();  
    }

    void OnEnable(){
        LoadFish();
        LoadAppQuitTime();
    }
    
    public void OnApplicationFocus(bool value){
        if (value)
        {  
            LoadFish();             //이까진 됐음.      - 잘 들고 온다.       
            LoadAppQuitTime();      //그래서 그만둔 시간만 들고옴.       - 잘들고 온다.            
        }
        else    //이탈 시 실행
        {
            SaveFish();
            SaveAppQuitTime();
        }
    }

    public void OnApplicationQuit()
    {
        SaveFish();
        SaveAppQuitTime();
    }

    public bool SaveAppQuitTime()           //나간 시간 저장
    {
        bool result = false;
        try
        {
            var appQuitTime = DateTime.Now.ToLocalTime().ToBinary().ToString();     
            PlayerPrefs.SetString("FishAppQuitTime", appQuitTime);          //AppQuitTime을 다르게 저장을 해야 된다.
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
            if (PlayerPrefs.HasKey("FishAppQuitTime"))      //처음에는 없어도 ㄱㅊ 노상관임.
            {
                var appQuitTime = string.Empty;
                appQuitTime = PlayerPrefs.GetString("FishAppQuitTime");                
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
