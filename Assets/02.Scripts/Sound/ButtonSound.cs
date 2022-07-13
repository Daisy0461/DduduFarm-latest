using System.Collections;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audioSource;
    ButtonSound[] other;
    public AudioClip[] clips;
    // private NativeAudioPointer[] sound;
    // private NativeAudioPointer dropSound;
    // private NativeSource test;

    private void Start(){
        if(Application.platform == RuntimePlatform.Android){
            // NativeAudio.Initialize();
            // sound[0] = NativeAudio.Load(clips[0]);
            // sound[1] = NativeAudio.Load(clips[1]);
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    public void PlaySound(int num=0)
    {
        #if UNITY_EDITOR
        audioSource.PlayOneShot(clips[num]);
        #endif

        if(Application.platform == RuntimePlatform.Android){
            // test.Play(sound[num]);
        }
    }
    
    public void PlaySoundForEnd(AudioClip clip)
    {

        audioSource.clip = clip;
        #if UNITY_EDITOR
        audioSource.Play();
        #endif

        if(Application.platform == RuntimePlatform.Android){
            // NativeAudio.Load(audioSource.clip);
        }

        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(DelayDestroy());
    }
}
