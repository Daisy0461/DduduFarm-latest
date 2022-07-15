using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Inventory : MonoBehaviour
{
    [Header("Info Popup - Output")]
    public GameObject popupOutput;
    public Text output_nameTxt;
    public Image output_iconImg;
    public Text output_noteTxt;
    public Text output_goldTxt;

    [Header("Info Popup - Cycle")]
    public GameObject popupCycle;
    public Text cycle_nameTxt;
    public Image cycle_iconImg;
    public Text cycle_noteTxt;
    public Text cycle_cycletimeTxt;
    public Image cycle_inputImg;
    public Text cycle_inputTxt;
    public Image cycle_outputImg;
    public Text cycle_outputTxt;

    [Header("Info Popup - Fish")]
    public GameObject popupFish;
    public Text fish_nameTxt;
    public Image fish_iconImg;
    public Text fish_noteTxt;
    public Text fish_satietyTxt;
    public Text fish_likeTxt;

    int selectCode;

    public void SelectPopup(GameObject slot)
    {
        selectCode = slot.GetComponentInChildren<InventoryItemInfo>().code;
        if (selectCode < (int)DataTable.Seed) // 200: 작물 250: 씨앗
            PopupOutput();
        else if (selectCode < (int)DataTable.Output) // 250: 씨앗 300: 가공물
            PopupCycle((int)DataTable.Seed);    
        else if (selectCode < (int)DataTable.FishEgg) // 400: 물고기 450: 물고기 알
            PopupFish();
        else if (selectCode < (int)DataTable.Gem) // 450: 물고기 알 700: 보석
            PopupCycle((int)DataTable.FishEgg);
        else if (selectCode < (int)DataTable.Money) // 700: 보석 10001:골드
            PopupOutput();
    }

    public void PopupOutput()
    {// 작물, 보석
        output_nameTxt.text = IM.GetInfo(selectCode).name;
        output_iconImg.sprite = Resources.Load<Sprite>(IM.GetInfo(selectCode).imgPath);
        output_noteTxt.text = IM.GetInfo(selectCode).note;
        output_goldTxt.text = IM.GetInfo(selectCode).sellCost.ToString();
        popupOutput.SetActive(true);
    }

    public void PopupCycle(int type)
    {// 씨앗, 물고기알
        cycle_nameTxt.text = IM.GetInfo(selectCode).name;
        cycle_iconImg.sprite = Resources.Load<Sprite>(IM.GetInfo(selectCode).imgPath);
        cycle_noteTxt.text = IM.GetInfo(selectCode).note;
        if (type == (int)DataTable.Seed)
        {
            int time = CM.GetInfo(selectCode-50).grow1Time + CM.GetInfo(selectCode-50).grow2Time;
            cycle_cycletimeTxt.text = "성장시간 : "
                    +time/60+"분 "+time%60+"초";
            cycle_outputTxt.text = IM.GetInfo(selectCode-50).name+
                                    " "+CM.GetInfo(selectCode-50).havestMin+" ~ "+CM.GetInfo(selectCode-50).havestMax+"개";
        }
        else if (type == (int)DataTable.FishEgg)
        {
            int time = FM.GetInfo(selectCode-50).grow1Time + FM.GetInfo(selectCode-50).grow2Time;
            cycle_cycletimeTxt.text = "성장시간 : "
                    + time/60+"분 "+time%60+"초";
            cycle_outputTxt.text = IM.GetInfo(selectCode-50).name+" 1개";
        }
        cycle_inputImg.sprite = Resources.Load<Sprite>(IM.GetInfo(selectCode).imgPath);
        cycle_inputTxt.text = IM.GetInfo(selectCode).name + " 1개";
        cycle_outputImg.sprite = Resources.Load<Sprite>(IM.GetInfo(selectCode-50).imgPath);
        popupCycle.SetActive(true);
    }

    public void PopupFish()
    {// 물고기
        fish_nameTxt.text = IM.GetInfo(selectCode).name;
        fish_iconImg.sprite = Resources.Load<Sprite>(IM.GetInfo(selectCode).imgPath);
        fish_noteTxt.text = IM.GetInfo(selectCode).note;
        fish_satietyTxt.text = "부여 포만감 : "+FM.GetInfo(selectCode).full;
        fish_likeTxt.text = "부여 호감도 : "+FM.GetInfo(selectCode).like;
        popupFish.SetActive(true);
    }
}
