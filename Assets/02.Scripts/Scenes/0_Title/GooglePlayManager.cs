using UnityEngine;
// 구글 플레이 연동
using GooglePlayGames;
using GooglePlayGames.BasicApi;
 
public class GooglePlayManager : MonoBehaviour
{
#if PLATFORM_ANDROID
    void Awake()
    {
        var config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    public void OnClickLogin()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate(success => {
                if (success)
                {
                    Debug.Log("Authentication successful");
                    string userInfo = "Username: " + Social.localUser.userName +
                        "\nUser ID: " + Social.localUser.id +
                        "\nIsUnderage: " + Social.localUser.underage;
                    Debug.Log(userInfo);
                }
                else
                    Debug.Log("Authentication failed");
            });
        }
    }
 
    public void OnClickLogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Debug.Log("Logout");
    }
#endif
}