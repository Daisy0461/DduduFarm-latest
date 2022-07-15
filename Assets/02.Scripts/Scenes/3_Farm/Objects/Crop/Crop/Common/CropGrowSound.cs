using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropGrowSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    public void PlayGrowSound(){
        audioSource.Play();
    }

    public void AudioAwakePlay(){
        audioSource.playOnAwake = true;
    }

    public void AudioAwakePlayFalse(){
        audioSource.playOnAwake = false;
    }
}
