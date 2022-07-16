using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemInform : MonoBehaviour
{
    [SerializeField] public ItemInfo info;
    public ShopItemSlotCreate SIUI;

    public void OnClickShopSlot()
    {   
        SIUI.PlayButtonSound();
        if (SIUI.selectedShopItem != null)
        {
            SIUI.selectedShopItem.GetComponent<Image>().color = Color.white;
        }
        if (SIUI.selectedShopItem == this)
        {
            SIUI.selectedShopItem = null;
            return ;
        }
        this.GetComponent<Image>().color = Color.yellow;
        SIUI.selectedShopItem = this;
        EncryptedPlayerPrefs.SetInt("ItemPrice", info.buyCost);
    }

    public void OnClickIcon_InfoPopup()
    {
        SIUI.audioSource.Play();
        
        if (info.code < (int)DataTable.Output) // 250: 씨앗 300: 가공물
            PopupCycle((int)DataTable.Seed);    
        else if (info.code < (int)DataTable.Gem) // 450: 물고기 알 700: 보석
            PopupCycle((int)DataTable.FishEgg);
        else if (info.code == (int)DataTable.Love)
            PopupLove();
        else
            Debug.Log("no match type");
    }

    public void PopupCycle(int type)
    {// 씨앗, 물고기알
        SIUI.cycle_nameTxt.text = info.name;
        SIUI.cycle_iconImg.sprite = Resources.Load<Sprite>(info.imgPath);
        SIUI.cycle_noteTxt.text = info.note;
        if (type == (int)DataTable.Seed)
        {
            int time = SIUI.CM.GetInfo(info.code-50).grow1Time + SIUI.CM.GetInfo(info.code-50).grow2Time;
            SIUI.cycle_cycletimeTxt.text = "성장시간 : "
                    +time/60+"분 "+time%60+"초";
            SIUI.cycle_outputTxt.text = SIUI.IM.GetInfo(info.code-50).name+
                                    " "+SIUI.CM.GetInfo(info.code-50).havestMin+" ~ "+SIUI.CM.GetInfo(info.code-50).havestMax+"개";
        }
        else if (type == (int)DataTable.FishEgg)
        {
            int time = SIUI.FM.GetInfo(info.code-50).grow1Time + SIUI.FM.GetInfo(info.code-50).grow2Time;
            SIUI.cycle_cycletimeTxt.text = "성장시간 : "
                    + time/60+"분 "+time%60+"초";
            SIUI.cycle_outputTxt.text = SIUI.IM.GetInfo(info.code-50).name+" 1개";
        }
        SIUI.cycle_inputImg.sprite = Resources.Load<Sprite>(info.imgPath);
        SIUI.cycle_inputTxt.text = info.name + " 1개";
        SIUI.cycle_outputImg.sprite = Resources.Load<Sprite>(SIUI.IM.GetInfo(info.code-50).imgPath);
        SIUI.popupCycle.SetActive(true);
    }

    public void PopupLove()
    {
        SIUI.user_nameTxt.text = info.name;
        SIUI.user_iconImg.sprite = Resources.Load<Sprite>(info.imgPath);
        SIUI.user_noteTxt.text = info.note;
        SIUI.popupUser.SetActive(true);
    }
}
