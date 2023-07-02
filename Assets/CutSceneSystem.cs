using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[System.Serializable]
public class CutScene
{
    public GameObject _image;
    public CutSceneText[] _texts;
    public GameObject[] _effects;
    public bool _isPageStart;
    public bool _isPopup;
}

public class CutSceneSystem : MonoBehaviour
{
    [SerializeField] private CutScene[] _cutScenes;
    [SerializeField] private CutScene[] _leaveCutScenes;
    [SerializeField] private CutScene[] _stayCutScenes;
    [SerializeField] private TwoButtonPopup _leavePopup;
    [SerializeField] private TwoButtonPopup _leaveCheckPopup;

    private List<GameObject> _cachedObject = new List<GameObject>();

    private async void Start() 
    {
        _cachedObject.Clear();
        await PlayCutScene(_cutScenes);
    }

    private async Task PlayCutScene(CutScene[] cutScenes)
    {
        foreach (var cutSceneItem in cutScenes)
        {
            if (cutSceneItem._isPageStart)
            {
                foreach (var image in _cachedObject)
                {
                    image.SetActive(false);
                }
                _cachedObject.Clear();
            }
            
            cutSceneItem._image.SetActive(true);
            _cachedObject.Add(cutSceneItem._image);

            if (cutSceneItem._isPopup)
            {
                _leavePopup.gameObject.SetActive(true);
                _leavePopup.SetAction(OnClickLeaveButton, OnClickStayButton);
            }

            for (var index = 0; index < cutSceneItem._texts.Length; index++)
            {
                if (cutSceneItem._effects.Length > index && cutSceneItem._effects[index] != null)
                {
                    cutSceneItem._effects[index].SetActive(true);
                    _cachedObject.Add(cutSceneItem._effects[index]);
                }
                cutSceneItem._texts[index].gameObject.SetActive(true);
                await cutSceneItem._texts[index].PlayCutSceneText();
            }

            if (cutSceneItem._texts.Length == 0)
            {
                await Task.Delay(500);
            }
        }
    }

    private async void LeaveAction()
    {
        await PlayCutScene(_leaveCutScenes);
        ResetGameData();
        Loading.LoadSceneHandle("Title");
    }

    private void ResetGameData()
    {
        SaveManager.DeleteSaveData();
        PlayerPrefs.DeleteAll();
    }

    private void OnClickLeaveButton()
    {
        _leaveCheckPopup.gameObject.SetActive(true);
        _leaveCheckPopup.SetAction(LeaveAction, ()=>{_leavePopup.gameObject.SetActive(true);});
    }

    private async void OnClickStayButton()
    {
        await PlayCutScene(_stayCutScenes);
        Loading.LoadSceneHandle("Title");
    }
}
