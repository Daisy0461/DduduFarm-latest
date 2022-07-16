using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCutScene_To_Build : MonoBehaviour
{

    public void MoveScene_To_Build()
    {// 튜토리얼 작업할 때 활성화하기
        // EncryptedPlayerPrefs.SetInt("FarmTutorial", 0);
        // Loading.LoadSceneHandle("FarmTutorial");
        Loading.LoadSceneHandle("Farm");
    }
}
