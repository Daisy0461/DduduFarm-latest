using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Profile : MonoBehaviour
{   // Profile - UserImageBG에 삽입된 스크립트
    public Image userDataProfileImage;          // 유저데이터 패널에 있는 프로필 이미지
    public Image profileImg;                    // 프로필이미지배경 아래에 있는 프로필 이미지
    public Outline selectedOutline;             // 프로필선택패널에 있는 현재 프로필로 선택되어 있는 이미지의 배경이미지 for outline
    private Image selectedImg;                  // 프로필선택패널에 있는, 현재 프로필로 선택되어 있는 이미지
    public GameObject redLight;                 // 확인하지 못한 새로운 프로필 이미지가 있을 때 뜨는 빨간불
    static public List<int> dduduList = new List<int>();    // 획득한 뚜두 타입 저장
    public int mainProfile;                                 // 현재 지정된 프로필 저장
    public List<ProfileItem> profileObjectList = new List<ProfileItem>();                // 할당되어 있는 프로필 이미지 오브젝트

    [Header("Visual")]
    public Text nicknameTxt;
    public Text propertyTxt;
    public Text dduduTxt;

    public void Save()
    {
        string saveProfileDatas = "";
        for (int i=0; i<profileObjectList.Count; i++)
        {
            saveProfileDatas += profileObjectList[i].isClicked;
            if (i < profileObjectList.Count - 1)
                saveProfileDatas += ",";
        }
        EncryptedPlayerPrefs.SetString("ProfileDatas", saveProfileDatas);
        EncryptedPlayerPrefs.SetInt("MainProfile", mainProfile);
        PlayerPrefs.Save();
    }

    public void Load()  // Start 에서 Load
    {
        var saveProfileDatas = EncryptedPlayerPrefs.GetString("ProfileDatas").Split(',');
        mainProfile = EncryptedPlayerPrefs.GetInt("MainProfile", 0);
        if (saveProfileDatas.Length == 11)
        {
            for (int i=0; i<profileObjectList.Count; i++)
                profileObjectList[i].isClicked = System.Convert.ToInt32(saveProfileDatas[i]);
        }
        if (profileObjectList[0].isClicked == 1)
            profileObjectList[0].transform.GetChild(1).gameObject.SetActive(false);
    }

    public void SetMainProfile()
    {
        Image img = profileObjectList[mainProfile].transform.GetChild(0).GetComponent<Image>();
        profileImg.sprite = img.sprite;
        userDataProfileImage.sprite = img.sprite;
    }

    public void SetUnlockProfile()
    {// 새로 가진 프로필을 해금하기 & 클릭하지 않은 게 있다면 빨간불 켜기
        for (int i=0; i<profileObjectList.Count; i++)
        {
            if (i==0 || dduduList.Contains(i-1))
            {
                profileObjectList[i].haveIt = true;                                          
                profileObjectList[i].GetDduduImg();   
                if (profileObjectList[i].isClicked == 1)
                    profileObjectList[i].transform.GetChild(1).gameObject.SetActive(false);
                else redLight.SetActive(true);
            }
        }
    }

    private void OnEnable() 
    {
        Load();
        SetUnlockProfile();
        SetText();
    }

    private void OnDisable() 
    {
        Save();
    }

    public void SetText()
    {
        nicknameTxt.text = UserInfo.GetUserName();
        propertyTxt.text = "재산 : " + string.Format("{0:#,##0}", ItemManager.Instance.GetData((int)DataTable.Money)?.amount ?? 0);
        dduduTxt.text = "보유 뚜두 수 : " + DduduManager.Instance.GetDataListCount();
    }

    public void OnclickImage(Outline outline)
    {
        if (outline.GetComponent<Image>().color == Color.grey)    // 획득하지 못한 프로필일 경우 리턴
        {
            Debug.Log("획득하지 못한 프로필입니다.");
            return;
        }
        if (selectedOutline)
            selectedOutline.enabled = false;
        outline.enabled = true;

        // 현재 선택한 이미지를 가지고 있기 -> 후에 확인 버튼을 누르면 해당 이미지를 대표이미지로 설정
        selectedOutline = outline;
        selectedImg = outline.transform.GetChild(0).GetComponent<Image>();    
        
        // 클릭했으니 무조건 빨간불 꺼버리기
        ProfileItem profileItem = outline.GetComponent<ProfileItem>();
        profileItem.isClicked = 1;
        profileItem.transform.GetChild(1).gameObject.SetActive(false);// 빨간불 그냥 꺼버리기
    }

    public void OnClickSetImg()
    {
        if (selectedImg == null) return ;       
        mainProfile = selectedOutline.GetComponent<ProfileItem>().id;
        EncryptedPlayerPrefs.SetInt("MainProfile", (int)mainProfile);
        SetMainProfile();
        for (int i=0; i<profileObjectList.Count; i++)
        {
            if (profileObjectList[i].haveIt && profileObjectList[i].isClicked == 0)
                return;
        }
        redLight.SetActive(false);
    }
}
