using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchCategoryUI : MonoBehaviour
{
    public GameObject researchUI;

    private void Awake()
    {
        if (researchUI.name.Equals("FarmUpgrade"))
            ActiveResearchTypeUIEvent();
    }

    private void UpdateResearchCategoryUI(Color color)
    {
        gameObject.GetComponent<Image>().color = color;
    }

    public void ActiveResearchTypeUIEvent()
    {
        GameObject[] researchCategoryUIList = GameObject.FindGameObjectsWithTag("ResearchCategory");

        foreach(GameObject researchCategoryUI in researchCategoryUIList)
        {
            researchCategoryUI.GetComponent<ResearchCategoryUI>().InactiveResearchTypeUI();
        }

        UpdateResearchCategoryUI(Color.white);

        researchUI.SetActive(true);
    }

    public void InactiveResearchTypeUI()
    {
        UpdateResearchCategoryUI(Color.gray);
        researchUI.SetActive(false);
    }
}
