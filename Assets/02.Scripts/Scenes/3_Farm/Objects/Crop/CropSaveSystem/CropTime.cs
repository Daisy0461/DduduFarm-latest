using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CropTime : CropSaveLoad      
{
    //이 이후에는 시간 관련 변수들
    private DateTime m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();  //이건 나가는 시간이고 꼭 필요함!;
    
    void Awake(){
        LoadAppQuitTime();  
    }
    
    //게임 초기화(Awake), 중간 이탈, 중간 복귀 시 실행되는 함수
    public void OnApplicationFocus(bool value){
        if (value)
        {  
            LoadCrop();             //이까진 됐음.      - 잘 들고 온다.       
            LoadAppQuitTime();      //그래서 그만둔 시간만 들고옴.       - 잘들고 온다.            
        }
        else    //이탈 시 실행
        {
            SaveCrop();
            SaveAppQuitTime();
        }
    }

    //게임 종료 시 실행되는 함수
    public void OnApplicationQuit()
    {
        SaveCrop();
        SaveAppQuitTime();
    }

    // public void OnApplicationPause(){
    //     SaveCrop();
    //     SaveAppQuitTime();
    // }

    public bool SaveAppQuitTime()           //나간 시간 저장
    {
        bool result = false;
        try
        {
            var appQuitTime = DateTime.Now.ToLocalTime().ToBinary().ToString();     
            PlayerPrefs.SetString("AppQuitTime", appQuitTime);
            PlayerPrefs.Save();

            //Debug.Log("Save AppQuitTime : " + DateTime.Now.ToLocalTime().ToString());
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
            if (PlayerPrefs.HasKey("AppQuitTime"))      //처음에는 없어도 ㄱㅊ 노상관임.
            {
                var appQuitTime = string.Empty;
                appQuitTime = PlayerPrefs.GetString("AppQuitTime");                
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime));      //바이너리 값을 불러온다.
                //Debug.Log("Load Saved m_AppQuitTime: "+ m_AppQuitTime);
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
