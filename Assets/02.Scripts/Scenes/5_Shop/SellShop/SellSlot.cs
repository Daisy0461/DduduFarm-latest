using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellSlot : MonoBehaviour
{
    public SellItemSlotCreate SI;
    public ItemData iData = null;
    public int bCode;

    public void OnClickItem() {
        SI.audioSource.Play();
        if (SI.selectedSlot != null)
        {
            SI.selectedSlot.GetComponent<Image>().color = Color.white;
        }
        if (SI.selectedSlot == this)
        {
            SI.selectedSlot = null;
            return ;
        }
        this.GetComponent<Image>().color = Color.yellow;
        SI.selectedSlot = this;
    }

    public void OnClickIcon_InfoPopup()
    {
        SI.audioSource.Play();
        if (bCode > 0)
            PopupBuilding();
        else if (iData.info.code < (int)DataTable.Seed) // 200: 작물 250: 씨앗
            PopupOutput();
        else if (iData.info.code < (int)DataTable.Output) // 250: 씨앗 300: 가공물
            PopupCycle((int)DataTable.Seed);    
        else if (iData.info.code < (int)DataTable.FishEgg) // 400: 물고기 450: 물고기 알
            PopupFish();
        else if (iData.info.code < (int)DataTable.Gem) // 450: 물고기 알 700: 보석
            PopupCycle((int)DataTable.FishEgg);
        else if (iData.info.code < (int)DataTable.Money) // 700: 보석 10001:골드
            PopupOutput();
    }

    public void PopupOutput()
    {// 작물, 보석
        SI.output_nameTxt.text = iData.info.name;
        SI.output_iconImg.sprite = Resources.Load<Sprite>(iData.info.imgPath);
        SI.output_noteTxt.text = iData.info.note;
        SI.output_goldTxt.text = iData.info.sellCost.ToString();
        SI.popupOutput.SetActive(true);
    }

    public void PopupCycle(int type)
    {// 씨앗, 물고기알
        SI.cycle_nameTxt.text = iData.info.name;
        SI.cycle_iconImg.sprite = Resources.Load<Sprite>(iData.info.imgPath);
        SI.cycle_noteTxt.text = iData.info.note;
        if (type == (int)DataTable.Seed)
        {
            int time = SI.CM.GetInfo(iData.info.code-50).grow1Time + SI.CM.GetInfo(iData.info.code-50).grow2Time;
            SI.cycle_cycletimeTxt.text = "성장시간 : "
                    +time/60+"분 "+time%60+"초";
            SI.cycle_outputTxt.text = SI.IM.GetInfo(iData.info.code-50).name+
                                    " "+SI.CM.GetInfo(iData.info.code-50).havestMin+" ~ "+SI.CM.GetInfo(iData.info.code-50).havestMax+"개";
        }
        else if (type == (int)DataTable.FishEgg)
        {
            int time = SI.FM.GetInfo(iData.info.code-50).grow1Time + SI.FM.GetInfo(iData.info.code-50).grow2Time;
            SI.cycle_cycletimeTxt.text = "성장시간 : "
                    + time/60+"분 "+time%60+"초";
            SI.cycle_outputTxt.text = SI.IM.GetInfo(iData.info.code-50).name+" 1개";
        }
        SI.cycle_inputImg.sprite = Resources.Load<Sprite>(iData.info.imgPath);
        SI.cycle_inputTxt.text = iData.info.name + " 1개";
        SI.cycle_outputImg.sprite = Resources.Load<Sprite>(SI.IM.GetInfo(iData.info.code-50).imgPath);
        SI.popupCycle.SetActive(true);
    }

    public void PopupFish()
    {// 물고기
        SI.fish_nameTxt.text = iData.info.name;
        SI.fish_iconImg.sprite = Resources.Load<Sprite>(iData.info.imgPath);
        SI.fish_noteTxt.text = iData.info.note;
        SI.fish_satietyTxt.text = "부여 포만감 : "+SI.FM.GetInfo(iData.info.code).full;
        SI.fish_likeTxt.text = "부여 호감도 : "+SI.FM.GetInfo(iData.info.code).like;
        SI.popupFish.SetActive(true);
    }

    public void PopupBuilding()
    {
        BuildingInfo info = SI.BM.GetInfo(bCode);
        if (bCode < (int)DataTable.Craft) // common
        {
            // Label 건물이름
            SI.common_nameTxt.text = info.name;
            // 아이콘
            SI.common_iconImg.sprite = Resources.Load<Sprite>(info.imgPath); 
            // 설명 - 건물이름
            SI.common_explainTxt.text = info.note;
            // 골드 생산 주기
            SI.common_goldCycleTxt.text = "골드 생산 주기 : " + info.cycleTime / 60 + " 분";
            // 골드 생산량
            SI.common_goldOutputTxt.text = info.outputAmount.ToString();

            SI.popupCommon.SetActive(true);
        }
        else // craft
        {
            // Label 건물 이름
            SI.craft_nameText.text = info.name;
            // 아이콘
            SI.craft_iconImg.sprite = Resources.Load<Sprite>(info.imgPath);
            // 설명 - 건물 이름
            SI.craft_explainTxt.text = info.note;
            // 작업 시간
            SI.craft_CycleTxt.text = "작업 시간 : " + info.cycleTime / 60 + " 분";
            // 포만도 소모량
            SI.craft_satietyTxt.text = "포만도 소모량 : " + info.requireFull;
            // 재료 이미지
            SI.craft_materialImg.sprite = Resources.Load<Sprite>(SI.IM.GetInfo(info.matId).imgPath);
            // 재료 이름 및 개수
            SI.craft_materialTxt.text = SI.IM.GetInfo(info.matId).name + " " + info.matAmount;
            // 가공물 이미지
            SI.craft_outputImg.sprite = Resources.Load<Sprite>(SI.IM.GetInfo(info.outputId).imgPath);
            // 가공물 이름 및 개수
            SI.craft_outputTxt.text = SI.IM.GetInfo(info.outputId).name + " " +  info.outputAmount;
            
            SI.popupCraft.SetActive(true);
        }
    }
}
