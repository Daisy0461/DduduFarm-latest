using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchCompleteUI : MonoBehaviour
{
    private ResearchUnit researchUnit;
    private ResearchInfo researchInfo;

    public GameObject researchImage;
    public GameObject researchItemNameText;
    public GameObject researchExplainText;
    public GameObject levelValueText;
    public GameObject effectValueText;

    public void ActiveResearchCompleteUI(ResearchUnit researchUnit)
    {
        gameObject.SetActive(true);

        SetResearchUnit(researchUnit);

        SetResearchItemImage();
        UpdateResearchCompleteUIText();
    }

    public void SetResearchUnit(ResearchUnit researchUnit)
    {
        this.researchUnit = researchUnit;
    }

    public void SetResearchItemImage()
    {
        researchImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(researchInfo.imgPath);
    }

    public void UpdateResearchCompleteUIText()
    {
        researchItemNameText.GetComponent<Text>().text = researchInfo.name + " " + researchInfo.level.ToString();
        researchExplainText.GetComponent<Text>().text = researchInfo.note;
        levelValueText.GetComponent<Text>().text = researchInfo.level.ToString();
        effectValueText.GetComponent<Text>().text = researchInfo.researchValue.ToString();
    }
}
