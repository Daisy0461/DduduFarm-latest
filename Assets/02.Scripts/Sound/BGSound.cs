using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGSound : MonoBehaviour
{
    public float duration = 1f;
    public AudioSource audioSource;
    public AudioClip[] bgmClips;
    public bool singleton;
    enum BGM { title, intro, farm, shop, fishfarm, lab };
    BGSound[] other;
    BGM oldBgm = BGM.title;

    private void Start() 
    {
        other = FindObjectsOfType<BGSound>();
        for (int i=0; i<other.Length; i++)
        {   // 다른 애가 싱글턴이라면 내가 파괴되어야 함
            if (other[i].singleton && other[i] != this)   
            {
                other[i].PlayFade();
                Destroy(this.gameObject);
            }
        }
        singleton = true;
        DontDestroyOnLoad(this);
        PlayFade();
        this.gameObject.name += " (Singleton)";
    }

    public void PlayFade()
    {
        StartCoroutine(SoundFade());
    }

    IEnumerator SoundFade()
    {
        BGM bgm = SelectClip();
        if (oldBgm != bgm)
        {
            float currentTime = 0;
            float start = audioSource.volume;
            while (currentTime < duration)
            {
                yield return null;
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, 0.0f, currentTime / duration);
            }
        }
        bgm = SelectClip();
        audioSource.clip = bgmClips[(int)bgm];
        audioSource.volume = 1;
        if (oldBgm != bgm || !audioSource.isPlaying)  // 새 노래 재생
        {
            audioSource.Play();
        }
        oldBgm = bgm;
    }

    BGM SelectClip()
    {
        BGM bgm;
        string name = SceneManager.GetActiveScene().name;
        switch (name)
        {
            case "Title" : bgm = BGM.title;
                break;
            case "StartCutScene" : bgm = BGM.intro;
                break;
            case "Farm" : bgm = BGM.farm;
                break;
            case "FishFarm" : bgm = BGM.farm;
                break;
            case "Shop" : bgm = BGM.shop;
                break;
            case "Lab" : bgm = BGM.farm;
                break;
            default : bgm = oldBgm;
                break;              
        }
        return bgm;
    }
}
