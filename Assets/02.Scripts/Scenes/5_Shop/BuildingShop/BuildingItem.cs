using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingItem : MonoBehaviour
{
    // 건설 상점에 생성되는 건설 아이템 패널
    public ConstructionUI CSUI;
    public BuildingInfo info;

    public void OnClickBuilding() {
        CSUI.audioSource.Play();
        if (CSUI.selectedBuilding != null)
        {
            CSUI.selectedBuilding.GetComponent<Image>().color = Color.white;
        }
        if (CSUI.selectedBuilding == this)
        {
            CSUI.selectedBuilding = null;
            return ;
        }
        this.GetComponent<Image>().color = Color.yellow;
        CSUI.selectedBuilding = this;
    }

    public void OnClickIcon_InfoPopup()
    {
        CSUI.audioSource.Play();
        if (info.code < (int)DataTable.Craft) // common
        {
            // Label 건물이름
            CSUI.common_nameText.text = info.name;
            // 아이콘
            CSUI.common_iconImg.sprite = Resources.Load<Sprite>(info.imgPath); 
            // 설명 - 건물이름
            CSUI.common_explainTxt.text = info.note;
            // 골드 생산 주기
            CSUI.goldCycleTxt.text = "골드 생산 주기 : " + info.cycleTime / 60 + " 분";
            // 골드 생산량
            CSUI.outputTxt.text = info.outputAmount.ToString();

            CSUI.popupCommon.SetActive(true);
        }
        else // craft
        {
            // Label 건물 이름
            CSUI.craft_nameText.text = info.name;
            // 아이콘
            CSUI.craft_iconImg.sprite = Resources.Load<Sprite>(info.imgPath);
            // 설명 - 건물 이름
            CSUI.craft_explainTxt.text = info.note;
            // 작업 시간
            CSUI.craftCycleTxt.text = "작업 시간 : " + info.cycleTime / 60 + " 분";
            // 포만도 소모량
            CSUI.satietyTxt.text = "포만도 소모량 : " + info.requireFull;
            // 재료 이미지
            CSUI.materialImg.sprite = Resources.Load<Sprite>(CSUI.IM.GetInfo(info.matId).imgPath);
            // 재료 이름 및 개수
            CSUI.materialTxt.text = CSUI.IM.GetInfo(info.matId).name + " " + info.matAmount;
            // 가공물 이미지
            CSUI.outputImg.sprite = Resources.Load<Sprite>(CSUI.IM.GetInfo(info.outputId).imgPath);
            // 가공물 이름 및 개수
            CSUI.outputTxt.text = CSUI.IM.GetInfo(info.outputId).name + " " +  info.outputAmount;
            
            CSUI.popupCraft.SetActive(true);
        }
    }
}
