using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Vector2 target;
    RectTransform rectTransform;
    AudioSource audioSource;

    private void Awake()
    {
        Screen.SetResolution(720, 1280, false);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(GoDown());    
    }

    IEnumerator GoDown()
    {
        Vector2 cur = this.transform.position;
        audioSource.Play();
        while (Mathf.Abs(rectTransform.anchoredPosition.y - target.y) > 0f)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, target, 0.05f);
            yield return null;
        }
    }

    public void PressToStart()
    {
        if (EncryptedPlayerPrefs.GetInt("ExIntro") == 0) {
            EncryptedPlayerPrefs.SetInt("FarmTutorial", 0);
            EncryptedPlayerPrefs.SetInt("ExIntro", 1);
            SceneManager.LoadScene("StartCutScene");
        } else {
            Loading.LoadSceneHandle("Farm");
        }
    }
}
