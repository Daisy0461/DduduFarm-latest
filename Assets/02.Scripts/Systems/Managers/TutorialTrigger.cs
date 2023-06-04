using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(TutorialTrigger))]
public class TutorialTriggerEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Reset Tutorial Trigger"))
        {
            ((TutorialTrigger)target).OnResetTutorialButtonClick();
        }
    }
}

#endif

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private TutorialPopup _tutorialPopup;
    
    private string sceneName;
    
    private void Start() 
    {
        sceneName = SceneManager.GetActiveScene().name;
        var isTutorialDone = PlayerPrefs.GetInt($"{sceneName}", 0);
        if (isTutorialDone != 0) return;

        // 튜토리얼 진행
        _tutorialPopup.Activate();
        PlayerPrefs.SetInt($"{sceneName}", 1);
    }

#if UNITY_EDITOR

    public void OnResetTutorialButtonClick()
    {
        PlayerPrefs.SetInt($"{sceneName}", 0);
    }

#endif
}
