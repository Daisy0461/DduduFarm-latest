using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ProfileItem : MonoBehaviour
{
    public int isClicked;          // 클릭한 적이 있나?    클릭한 적이 없다면 하트 활성화
    public bool haveIt;             // 가지고 있나? 가지고 있지 않다면 회색 처리
    public int id;
    
    public void GetDduduImg()
    {// 새로운 뚜두 획득으로 인한 뚜두 프로필 획득
        // 이미지 색 white, 빨간불 
        this.haveIt = true;
        this.GetComponent<Image>().color = Color.white;
        this.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        this.transform.GetChild(1).gameObject.SetActive(true);
    }
}