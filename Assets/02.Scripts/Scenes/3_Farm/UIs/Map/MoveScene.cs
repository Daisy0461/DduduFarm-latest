using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [SerializeField] private CropTime cropTime;
    [SerializeField] private FishTime fishTime;

    public void BuildToOtherScene(string sceneName)
    {
        cropTime.SaveCrop();
        cropTime.SaveAppQuitTime();
        DduduManager.Instance.Save();
        Loading.LoadSceneHandle(sceneName);
    }

    public void GoToBuild()
    {
        SceneManager.LoadScene("Farm");
    }

    public void FishToBuild()
    {    
        fishTime.SaveFish();
        fishTime.SaveAppQuitTime();
        SceneManager.LoadScene("Farm");
    }

    public void LabToBuild()
    {
        SceneManager.LoadScene("Farm");
    }
}
