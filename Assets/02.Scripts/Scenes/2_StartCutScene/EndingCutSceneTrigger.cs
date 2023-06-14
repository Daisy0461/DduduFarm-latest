using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCutSceneTrigger : MonoBehaviour
{
    private void OnEnable() 
    {
        Loading.LoadSceneHandle("EndingCutScene");
    }
}
