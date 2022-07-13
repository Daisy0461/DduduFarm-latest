using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileInit : MonoBehaviour
{//userData-userDataPanel-Profile 에 위치
    public Profile profile;

    void Start()
    {
        profile.Load();
        profile.SetMainProfile();
    }
}
