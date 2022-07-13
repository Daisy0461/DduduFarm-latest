using System.Collections;
using UnityEngine;

public class SoundFadeInOut : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    public float duration;
    public float maxValue = 1;

    void Start(){
        StartCoroutine(AudioFadeIn());
    }

    public IEnumerator AudioFadeOut()
    {
        //사운드 점점 작아짐.
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, 0.0f, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public IEnumerator AudioFadeIn()
    {
        //사운드 점점 커짐
        float currentTime = 0;

        while (currentTime < duration){
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, 1.0f, currentTime / duration);
            if (audioSource.volume >= maxValue) break;
            yield return null;
        }
        yield break;
    }
}
