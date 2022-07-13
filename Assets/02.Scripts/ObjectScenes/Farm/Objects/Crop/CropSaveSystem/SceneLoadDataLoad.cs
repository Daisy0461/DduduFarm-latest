using UnityEngine;
using System;

public class SceneLoadDataLoad : MonoBehaviour
{
    [SerializeField]
    private CropTime cropTime;
    
    void OnEnable(){
        cropTime.LoadCrop();
        cropTime.LoadAppQuitTime();
    }

    private void OnDestroy() 
    {
        var appQuitTime = DateTime.Now.ToLocalTime().ToBinary().ToString(); 
        PlayerPrefs.SetString("AppQuitTime", appQuitTime); 
    }
}
