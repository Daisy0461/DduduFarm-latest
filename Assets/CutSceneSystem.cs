using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CutScene
{
    public GameObject _image;
    public CutSceneText[] _texts;
    public GameObject[] _effects;
    public bool _isPageStart;
}

public class CutSceneSystem : MonoBehaviour
{
    [SerializeField] private CutScene[] _cutScenes;

    private List<GameObject> _cachedObject = new List<GameObject>();

    private void Start() 
    {
        _cachedObject.Clear();
        PlayCutScene();
    }

    private async void PlayCutScene()
    {
        foreach (var cutSceneItem in _cutScenes)
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
            for (var index = 0; index < cutSceneItem._texts.Length; index++)
            {Debug.Log("here");
                if (cutSceneItem._effects.Length > index && cutSceneItem._effects[index] != null)
                {
                    cutSceneItem._effects[index].SetActive(true);
                    _cachedObject.Add(cutSceneItem._effects[index]);
                }
                cutSceneItem._texts[index].gameObject.SetActive(true);
                await cutSceneItem._texts[index].PlayCutSceneText();
            }
        }
    }
}
