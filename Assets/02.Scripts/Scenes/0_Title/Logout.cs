using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logout : MonoBehaviour
{
    public GameObject Login;
    public GameObject PressText;

    public void OnClickLogout()
    {
        EncryptedPlayerPrefs.SetInt("ExIntro", 0);
        switch (UserInfo.loginMethod)
        {
            case "google" : GoogleLogout(); break;
            case "guest" : break;
        }
        UserInfo.SetUserName(string.Empty, string.Empty);
        Login.SetActive(true);
        PressText.SetActive(false);
        this.gameObject.SetActive(false);
    }

    void GoogleLogout()
    {
        #if PLATFORM_ANDROID
        GPGSBinder.Inst.Logout();
        #endif
    }
}
