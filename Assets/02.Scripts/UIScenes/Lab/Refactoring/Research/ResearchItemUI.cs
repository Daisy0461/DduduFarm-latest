using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchItemUI : MonoBehaviour
{
    private const int NO_REQUIRE_RESEARCH = 0;

    private int researchID;

    public Sprite researchItemCompleteImage;
    private Sprite researchItemImage;

    public GameObject researchProgressUI;
    public GameObject researchCompleteUI;

    private void Awake()
    {
        gameObject.GetComponent<ResearchUnit>().SetResearchUnitFromInfoDict();

        researchID = gameObject.GetComponent<ResearchUnit>().researchID;
        researchItemImage = Resources.Load<Sprite>(gameObject.GetComponent<ResearchUnit>().researchInfo.imgPath);

        gameObject.GetComponent<Image>().sprite = researchItemImage;

        gameObject.transform.Find("Text").GetComponent<Text>().text = gameObject.GetComponent<ResearchUnit>().researchInfo.name + gameObject.GetComponent<ResearchUnit>().researchInfo.level.ToString();
    }

    private void OnEnable()
    {
        UpdateResearchItemUI();
    }

    public void UpdateResearchItemUI()
    {
        bool isResearched = ResearchManager.Instance.GetDataList().Contains(ResearchManager.Instance.GetData(researchID));

        if (isResearched)
        {
            gameObject.GetComponent<Image>().sprite = researchItemCompleteImage;
        }

        else
        {
            gameObject.GetComponent<Image>().sprite = researchItemImage;
            gameObject.GetComponent<Image>().color = CheckRequireResearch(researchID);
        }
    }

    public Color CheckRequireResearch(int researchID)
    {
        int requireResearchID = gameObject.GetComponent<ResearchUnit>().researchInfo.preCode;

        if (requireResearchID == NO_REQUIRE_RESEARCH)
        {
            return Color.white;
        }

        else if (ResearchManager.Instance.GetDataList().Contains(ResearchManager.Instance.GetData(gameObject.GetComponent<ResearchUnit>().researchInfo.preCode)))
        {
            return Color.white;
        }

        else
        {
            return Color.gray;
        }
    }

    // gray는 선행 연구가 진행되지 않은 항목
    public void ResearchItemUIEvent()
    {
        if (gameObject.GetComponent<Image>().color == Color.white)
        {
            ActiveProperResearchUI();
        }
    }

    public void ActiveProperResearchUI()
    {
        bool isResearched = ResearchManager.Instance.GetDataList().Contains(ResearchManager.Instance.GetData(researchID));

        if (isResearched)
        {
            researchCompleteUI.GetComponent<ResearchCompleteUI>().ActiveResearchCompleteUI(gameObject.GetComponent<ResearchUnit>());
        }

        else
        {
            researchProgressUI.GetComponent<ResearchProgressUI>().ActiveResearchProgressUI(gameObject.GetComponent<ResearchUnit>());
        }
    }
}
