using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Login : MonoBehaviour
{
    public GameObject LogIn;
    public GameObject LoadingBar;
    public GameObject PressText;
    public GameObject Logout;

    private void Start() 
    {
        if (UserInfo.GetUserName() != null)
        {
            switch (UserInfo.loginMethod)
            {
                case "google" : GoogleLogin(); break;
                case "guest" : GuestLogin(); break;
            }
        }
    }

    public void GoogleLogin()
    {
#if PLATFORM_ANDROID
        string ran = (new DateTime(1970, 1, 1).ToLocalTime()).ToString();
        UserInfo.SetUserName($"user{ran}", "google");
        // 안드로이드로 플랫폼을 바꿀 때 주석처리 풀기
        GPGSBinder.Inst.Login((success, localUser) => UserInfo.userName = $"{localUser.userName}");
        LoginPass();
#endif
    }

    public void GuestLogin()
    {
        string ran = (new DateTime(1970, 1, 1).ToLocalTime()).ToString();
        UserInfo.SetUserName("Guest", "guest");
        LoginPass();
    }

    public void LoginPass()
    {
        LogIn.SetActive(false);
        LoadingBar.SetActive(true);
        // 게임 시작 시 리소스 다운 및 업데이트
        // http://devkorea.co.kr/bbs/board.php?bo_table=m03_qna&wr_id=48962
        // 메서드 필요
        StartCoroutine(LoadResource());
    }

    IEnumerator LoadResource()
    {
        // 다운 받을 데이터 - 유저 자산, 해금 상태, 연구 상태, 뚜두 관련, 튜토리얼+인트로 경험 
        // 리소스 다운 완료 시 로딩 바 비활성화 및 PressText 활성화
        if (EncryptedPlayerPrefs.GetInt("ResourceCheck", 0) == 0)   // LiveOps를 더 조사해서 적용하기
        {
            // 유저 자산
            ItemManager.Instance.AddData((int)DataTable.Money, 100000);
            // EncryptedPlayerPrefs.SetInt("Money", 10000);

            // 해금
            EncryptedPlayerPrefs.SetInt("FarmUnlock_1", 1); // 2번 농장바닥 해금
            EncryptedPlayerPrefs.SetInt("FishFarmUnlock_0", 1); // 1번 양식장 해금

            EncryptedPlayerPrefs.SetInt("ResourceCheck", 1);
        }
        LoadingBar.SetActive(false);
        PressText.SetActive(true);
        Logout.SetActive(true);
        yield return null;
    }
}
