using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCutScene_To_Build : MonoBehaviour
{
    public void MoveScene_To_Build()
    {
        Loading.LoadSceneHandle("Farm");
    }
}
