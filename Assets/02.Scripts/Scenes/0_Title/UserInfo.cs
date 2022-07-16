using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserInfo
{// 유저, 세션 정보 관리
    public static string userName;
    public static string loginMethod;

    public static void SetUserName(string userName, string loginMethod)
    {
        UserInfo.userName = userName;
        UserInfo.loginMethod = loginMethod;
        EncryptedPlayerPrefs.SetString("userName", UserInfo.userName);
        EncryptedPlayerPrefs.SetString("loginMethod", UserInfo.loginMethod);
    }

    public static string GetUserName()
    {
        userName = EncryptedPlayerPrefs.GetString("userName");
        loginMethod = EncryptedPlayerPrefs.GetString("loginMethod");
        if (userName == null || userName == "") return null;
        else return userName;
    }
}
