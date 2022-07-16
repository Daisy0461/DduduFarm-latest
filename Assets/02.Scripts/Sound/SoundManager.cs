using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioMixer mixer;
    public bool masterMute, BGMute, SFXMute;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider BGSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private GameObject masterButtonOnImage, masterButtonOffImage, bgButtonOnImage, bgButtonOffImage, sfxButtonOnImage, sfxButtonOffImage;
    [SerializeField] private GameObject masterButtonOn, masterButtonOff, bgButtonOn, bgButtonOff, sfxButtonOn, sfxButtonOff;

    public void Start(){
        if(SceneManager.GetActiveScene().name != "Farm"){
            OtherSceneSoundLoad();
        }else if(EncryptedPlayerPrefs.HasKey("MasterSoundVolume")){
            BuildSoundLoad();        //MasterMute여기서는 값이 바뀐채로 적용이 된다.
        }else{
            masterSlider.value = EncryptedPlayerPrefs.GetFloat("MasterSoundVolume", 1.0f);      mixer.SetFloat("MasterSoundVolume", Mathf.Log10(masterSlider.value) * 20);
            BGSlider.value = EncryptedPlayerPrefs.GetFloat("BGSoundVolume", 1.0f);              mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSlider.value) * 20);
            SFXSlider.value = EncryptedPlayerPrefs.GetFloat("SFXSoundVolume", 1.0f);            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(SFXSlider.value) * 20);

            masterButtonOn.SetActive(false);    bgButtonOn.SetActive(false);        sfxButtonOn.SetActive(false);       //On이면 이게 false
            masterButtonOff.SetActive(true);    bgButtonOff.SetActive(true);        sfxButtonOff.SetActive(true);   //off면 이게 false
            masterButtonOnImage.SetActive(true);   bgButtonOnImage.SetActive(true);   sfxButtonOnImage.SetActive(true);  //위와 반대
            masterButtonOffImage.SetActive(false);   bgButtonOffImage.SetActive(false);  sfxButtonOffImage.SetActive(false);
        }
    }

    private void BuildSoundLoad(){
        if(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") != -80.0f)
        {
            //all not mute
            masterSlider.value = EncryptedPlayerPrefs.GetFloat("MasterSoundVolume", 1.0f);      mixer.SetFloat("MasterSoundVolume", Mathf.Log10(masterSlider.value) * 20);
            BGSlider.value = EncryptedPlayerPrefs.GetFloat("BGSoundVolume", 1.0f);              mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSlider.value) * 20);
            SFXSlider.value = EncryptedPlayerPrefs.GetFloat("SFXSoundVolume", 1.0f);            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(SFXSlider.value) * 20);

            masterButtonOn.SetActive(false);    bgButtonOn.SetActive(false);        sfxButtonOn.SetActive(false);       
            masterButtonOff.SetActive(true);    bgButtonOff.SetActive(true);        sfxButtonOff.SetActive(true);   
            masterButtonOnImage.SetActive(true);   bgButtonOnImage.SetActive(true);   sfxButtonOnImage.SetActive(true);  
            masterButtonOffImage.SetActive(false);   bgButtonOffImage.SetActive(false);  sfxButtonOffImage.SetActive(false);
        }else if (EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") != -80.0f){
            //master mute
            masterMute = true;
            masterSlider.value = EncryptedPlayerPrefs.GetFloat("SavedMasterSoundVolume");       mixer.SetFloat("MasterSoundVolume", -80.0f);
            BGSlider.value = EncryptedPlayerPrefs.GetFloat("BGSoundVolume", 1.0f);              mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSlider.value) * 20);
            SFXSlider.value = EncryptedPlayerPrefs.GetFloat("SFXSoundVolume", 1.0f);            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(SFXSlider.value) * 20);

            masterButtonOn.SetActive(true);    bgButtonOn.SetActive(false);        sfxButtonOn.SetActive(false);       
            masterButtonOff.SetActive(false);    bgButtonOff.SetActive(true);        sfxButtonOff.SetActive(true);   
            masterButtonOnImage.SetActive(false);   bgButtonOnImage.SetActive(true);   sfxButtonOnImage.SetActive(true);  
            masterButtonOffImage.SetActive(true);   bgButtonOffImage.SetActive(false);  sfxButtonOffImage.SetActive(false);
        }else if (EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") != -80.0f){
            //BG mute
            BGMute = true;
            masterSlider.value = EncryptedPlayerPrefs.GetFloat("MasterSoundVolume", 1.0f);      mixer.SetFloat("MasterSoundVolume", Mathf.Log10(masterSlider.value) * 20);
            BGSlider.value = EncryptedPlayerPrefs.GetFloat("SavedBGSoundVolume");               mixer.SetFloat("BGSoundVolume", -80.0f);
            SFXSlider.value = EncryptedPlayerPrefs.GetFloat("SFXSoundVolume", 1.0f);            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(SFXSlider.value) * 20);

            masterButtonOn.SetActive(false);    bgButtonOn.SetActive(true);        sfxButtonOn.SetActive(false);       
            masterButtonOff.SetActive(true);    bgButtonOff.SetActive(false);        sfxButtonOff.SetActive(true);   
            masterButtonOnImage.SetActive(true);   bgButtonOnImage.SetActive(false);   sfxButtonOnImage.SetActive(true);  
            masterButtonOffImage.SetActive(false);   bgButtonOffImage.SetActive(true);  sfxButtonOffImage.SetActive(false);
        }else if (EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") == -80.0f){
            //SFX mute
            SFXMute = true;
            masterSlider.value = EncryptedPlayerPrefs.GetFloat("MasterSoundVolume", 1.0f);      mixer.SetFloat("MasterSoundVolume", Mathf.Log10(masterSlider.value) * 20);
            BGSlider.value = EncryptedPlayerPrefs.GetFloat("BGSoundVolume", 1.0f);              mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSlider.value) * 20);
            SFXSlider.value = EncryptedPlayerPrefs.GetFloat("SavedSFXSoundVolume");             mixer.SetFloat("SFXSoundVolume", -80.0f);

            masterButtonOn.SetActive(false);    bgButtonOn.SetActive(false);        sfxButtonOn.SetActive(true);       
            masterButtonOff.SetActive(true);    bgButtonOff.SetActive(true);        sfxButtonOff.SetActive(false);   
            masterButtonOnImage.SetActive(true);   bgButtonOnImage.SetActive(true);   sfxButtonOnImage.SetActive(false);  
            masterButtonOffImage.SetActive(false);   bgButtonOffImage.SetActive(false);  sfxButtonOffImage.SetActive(true);
        }else if (EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") != -80.0f){
            //master & BG mute
            masterMute = true;  BGMute = true;
            masterSlider.value = EncryptedPlayerPrefs.GetFloat("SavedMasterSoundVolume");       mixer.SetFloat("MasterSoundVolume", -80.0f);
            BGSlider.value = EncryptedPlayerPrefs.GetFloat("SavedBGSoundVolume");               mixer.SetFloat("BGSoundVolume", -80.0f);
            SFXSlider.value = EncryptedPlayerPrefs.GetFloat("SFXSoundVolume", 1.0f);            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(SFXSlider.value) * 20);

            masterButtonOn.SetActive(true);    bgButtonOn.SetActive(true);        sfxButtonOn.SetActive(false);       
            masterButtonOff.SetActive(false);    bgButtonOff.SetActive(false);        sfxButtonOff.SetActive(true);   
            masterButtonOnImage.SetActive(false);   bgButtonOnImage.SetActive(false);   sfxButtonOnImage.SetActive(true);  
            masterButtonOffImage.SetActive(true);   bgButtonOffImage.SetActive(true);  sfxButtonOffImage.SetActive(false);
        }else if (EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") == -80.0f){
            //master & SFX mute
            masterMute = true;  SFXMute = true;
            masterSlider.value = EncryptedPlayerPrefs.GetFloat("SavedMasterSoundVolume");       mixer.SetFloat("MasterSoundVolume", -80.0f);
            BGSlider.value = EncryptedPlayerPrefs.GetFloat("BGSoundVolume", 1.0f);              mixer.SetFloat("BGSoundVolume", Mathf.Log10(BGSlider.value) * 20);
            SFXSlider.value = EncryptedPlayerPrefs.GetFloat("SavedSFXSoundVolume");             mixer.SetFloat("SFXSoundVolume", -80.0f);

            masterButtonOn.SetActive(true);    bgButtonOn.SetActive(false);        sfxButtonOn.SetActive(true);       
            masterButtonOff.SetActive(false);    bgButtonOff.SetActive(true);        sfxButtonOff.SetActive(false);   
            masterButtonOnImage.SetActive(false);   bgButtonOnImage.SetActive(true);   sfxButtonOnImage.SetActive(false);  
            masterButtonOffImage.SetActive(true);   bgButtonOffImage.SetActive(false);  sfxButtonOffImage.SetActive(true);
        }else if (EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") == -80.0f){
            //BG & SFX mute
            BGMute = true;  SFXMute = true;
            masterSlider.value = EncryptedPlayerPrefs.GetFloat("MasterSoundVolume", 1.0f);      mixer.SetFloat("MasterSoundVolume", Mathf.Log10(masterSlider.value) * 20);
            BGSlider.value = EncryptedPlayerPrefs.GetFloat("SavedBGSoundVolume");               mixer.SetFloat("BGSoundVolume", -80.0f);
            SFXSlider.value = EncryptedPlayerPrefs.GetFloat("SavedSFXSoundVolume");             mixer.SetFloat("SFXSoundVolume", -80.0f);

            masterButtonOn.SetActive(false);    bgButtonOn.SetActive(true);        sfxButtonOn.SetActive(true);       
            masterButtonOff.SetActive(true);    bgButtonOff.SetActive(false);        sfxButtonOff.SetActive(false);   
            masterButtonOnImage.SetActive(true);   bgButtonOnImage.SetActive(false);   sfxButtonOnImage.SetActive(false);  
            masterButtonOffImage.SetActive(false);   bgButtonOffImage.SetActive(true);  sfxButtonOffImage.SetActive(true);
        }else if (EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") == -80.0f){
            //all mute
            masterMute = true;  BGMute = true;  SFXMute = true;
            masterSlider.value = EncryptedPlayerPrefs.GetFloat("SavedMasterSoundVolume");       mixer.SetFloat("MasterSoundVolume", -80.0f);
            BGSlider.value = EncryptedPlayerPrefs.GetFloat("SavedBGSoundVolume");               mixer.SetFloat("BGSoundVolume", -80.0f);
            SFXSlider.value = EncryptedPlayerPrefs.GetFloat("SavedSFXSoundVolume");             mixer.SetFloat("SFXSoundVolume", -80.0f);

            masterButtonOn.SetActive(true);    bgButtonOn.SetActive(true);        sfxButtonOn.SetActive(true);       
            masterButtonOff.SetActive(false);    bgButtonOff.SetActive(false);        sfxButtonOff.SetActive(false);   
            masterButtonOnImage.SetActive(false);   bgButtonOnImage.SetActive(false);   sfxButtonOnImage.SetActive(false);  
            masterButtonOffImage.SetActive(true);   bgButtonOffImage.SetActive(true);  sfxButtonOffImage.SetActive(true);
        }
    }

    private void OtherSceneSoundLoad(){
        if(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") != -80.0f){
            //all not mute
            mixer.SetFloat("MasterSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume")) * 20);
            mixer.SetFloat("BGSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("BGSoundVolume")) * 20);
            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("SFXSoundVolume")) * 20);
        }else if(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") != -80.0f){
            //master mute
            mixer.SetFloat("MasterSoundVolume", -80.0f);
            mixer.SetFloat("BGSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("BGSoundVolume")) * 20);
            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("SFXSoundVolume")) * 20);
        }else if(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") != -80.0f){
            //BG mute
            mixer.SetFloat("MasterSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume")) * 20);
            mixer.SetFloat("BGSoundVolume", -80.0f);
            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("SFXSoundVolume")) * 20);
        }else if(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") == -80.0f){
            //sfx mute
            mixer.SetFloat("MasterSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume")) * 20);
            mixer.SetFloat("BGSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("BGSoundVolume")) * 20);
            mixer.SetFloat("SFXSoundVolume", -80.0f);
        }else if(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") != -80.0f){
            //master & bg mute
            mixer.SetFloat("MasterSoundVolume", -80.0f);
            mixer.SetFloat("BGSoundVolume", -80.0f);
            mixer.SetFloat("SFXSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("SFXSoundVolume")) * 20);
        }else if(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") == -80.0f){
            //master & sfx mute
            mixer.SetFloat("MasterSoundVolume", -80.0f);
            mixer.SetFloat("BGSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("BGSoundVolume")) * 20);
            mixer.SetFloat("SFXSoundVolume", -80.0f);
        }else if(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") != -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") == -80.0f){
            //bg & sfx mute
            mixer.SetFloat("MasterSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume")) * 20);
            mixer.SetFloat("BGSoundVolume", -80.0f);
            mixer.SetFloat("SFXSoundVolume", -80.0f);
        }else if(EncryptedPlayerPrefs.GetFloat("MasterSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("BGSoundVolume") == -80.0f && EncryptedPlayerPrefs.GetFloat("SFXSoundVolume") == -80.0f){
            mixer.SetFloat("MasterSoundVolume", -80.0f);
            mixer.SetFloat("BGSoundVolume", -80.0f);
            mixer.SetFloat("SFXSoundVolume", -80.0f);
        }
    }

    public void MasterSoundVolume(float val)        //조절은 됌. 그리고 여기에 On(버튼과 이미지 활성화하는거)도 추가해야함 일단 나중에 하자
    {
        mixer.SetFloat("MasterSoundVolume", Mathf.Log10(val) * 20);
        EncryptedPlayerPrefs.SetFloat("MasterSoundVolume", val);

        if(masterMute == true){
            MasterOnOffActive();
            masterMute = false;
        }
    }

    public void MasterSoundMute(){
        masterMute = true;
        EncryptedPlayerPrefs.SetFloat("SavedMasterSoundVolume", EncryptedPlayerPrefs.GetFloat("MasterSoundVolume"));
        mixer.SetFloat("MasterSoundVolume", -80.0f);
        EncryptedPlayerPrefs.SetFloat("MasterSoundVolume", -80.0f);

        masterButtonOn.SetActive(true);             masterButtonOff.SetActive(false);
        masterButtonOnImage.SetActive(false);       masterButtonOffImage.SetActive(true);
    }

    public void MasterSoundUnlookMute(){
        masterMute = false;
        mixer.SetFloat("MasterSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("SavedMasterSoundVolume")) * 20);
        EncryptedPlayerPrefs.SetFloat("MasterSoundVolume", EncryptedPlayerPrefs.GetFloat("SavedMasterSoundVolume"));

        masterButtonOn.SetActive(false);             masterButtonOff.SetActive(true);
        masterButtonOnImage.SetActive(true);         masterButtonOffImage.SetActive(false);
    }

    public void MasterOnOffActive()
    {
        masterButtonOn.SetActive(false);             masterButtonOff.SetActive(true);
        masterButtonOnImage.SetActive(true);         masterButtonOffImage.SetActive(false);
    }

    public void BGSoundVolume(float val)
    {
        mixer.SetFloat("BGSoundVolume", Mathf.Log10(val) * 20);
        EncryptedPlayerPrefs.SetFloat("BGSoundVolume", val);
        if(BGMute == true){
            BGOnOffActive();
            BGMute = false;
        }
    }

    public void BGSoundMute(){
        BGMute = true;
        EncryptedPlayerPrefs.SetFloat("SavedBGSoundVolume", EncryptedPlayerPrefs.GetFloat("BGSoundVolume"));
        mixer.SetFloat("BGSoundVolume", -80.0f);
        EncryptedPlayerPrefs.SetFloat("BGSoundVolume", -80.0f);
        
        bgButtonOn.SetActive(true);             bgButtonOff.SetActive(false);
        bgButtonOnImage.SetActive(false);       bgButtonOffImage.SetActive(true);
    }

    public void BGSoundUnlookMute(){
        BGMute = false;
        mixer.SetFloat("BGSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("SavedBGSoundVolume")) * 20);
        EncryptedPlayerPrefs.SetFloat("BGSoundVolume", EncryptedPlayerPrefs.GetFloat("SavedBGSoundVolume"));

        bgButtonOn.SetActive(false);             bgButtonOff.SetActive(true);
        bgButtonOnImage.SetActive(true);         bgButtonOffImage.SetActive(false);
    }

    public void BGOnOffActive()
    {
        bgButtonOn.SetActive(false);             bgButtonOff.SetActive(true);
        bgButtonOnImage.SetActive(true);         bgButtonOffImage.SetActive(false);
    }

    public void SFXSoundVolume(float val)
    {
        mixer.SetFloat("SFXSoundVolume", Mathf.Log10(val) * 20);
        EncryptedPlayerPrefs.SetFloat("SFXSoundVolume", val);
        if(SFXMute == true){
            SFXOnOffActive();
            SFXMute = false;
        }
    }

    public void SFXSoundMute(){
        SFXMute = true;
        EncryptedPlayerPrefs.SetFloat("SavedSFXSoundVolume", EncryptedPlayerPrefs.GetFloat("SFXSoundVolume"));
        mixer.SetFloat("SFXSoundVolume", -80.0f);
        EncryptedPlayerPrefs.SetFloat("SFXSoundVolume", -80.0f);

        sfxButtonOn.SetActive(true);             sfxButtonOff.SetActive(false);
        sfxButtonOnImage.SetActive(false);       sfxButtonOffImage.SetActive(true);
    }

    public void SFXSoundUnlookMute(){  
        SFXMute = false;
        mixer.SetFloat("SFXSoundVolume", Mathf.Log10(EncryptedPlayerPrefs.GetFloat("SavedSFXSoundVolume")) * 20);
        EncryptedPlayerPrefs.SetFloat("SFXSoundVolume", EncryptedPlayerPrefs.GetFloat("SavedSFXSoundVolume"));

        sfxButtonOn.SetActive(false);             sfxButtonOff.SetActive(true);
        sfxButtonOnImage.SetActive(true);         sfxButtonOffImage.SetActive(false);
    }

    public void SFXOnOffActive()
    {
        sfxButtonOn.SetActive(false);             sfxButtonOff.SetActive(true);
        sfxButtonOnImage.SetActive(true);         sfxButtonOffImage.SetActive(false);
    }
}