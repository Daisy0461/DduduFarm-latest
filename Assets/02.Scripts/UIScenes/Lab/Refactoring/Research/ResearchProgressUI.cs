using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchProgressUI : MonoBehaviour
{
    public struct Coordinate
    {
        private float posX;
        private float posY;

        public Coordinate(float posX, float posY)
        {
            this.posX = posX;
            this.posY = posY;
        }

        public float getX()
        {
            return posX;
        }

        public float getY()
        {
            return posY;
        }
    }

    private ResearchUnit researchUnit;
    private ResearchInfo researchInfo;

    public List<Coordinate>[] researchMaterialCoordinateList = new List<Coordinate>[3];

    public GameObject researchImage;
    public GameObject researchItemNameText;
    public GameObject researchExplainText;
    public GameObject levelValueText;
    public GameObject effectValueText;

    public GameObject[] requireMaterialSlotList = new GameObject[3];

    [SerializeField]
    private bool canResearch;

    // grayï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿?ï¿½ï¿½ï¿½ï¿½ ï¿½×¸ï¿½
    private void Awake()
    {
        researchMaterialCoordinateList[0] = new List<Coordinate>() { new Coordinate(0, -240) };
        researchMaterialCoordinateList[1] = new List<Coordinate>() { new Coordinate(-100, -240), new Coordinate(100, -240) };
        researchMaterialCoordinateList[2] = new List<Coordinate>() { new Coordinate(-175, -240), new Coordinate(0, -240), new Coordinate(175, -240) };
    }

    private void OnEnable()
    {
        canResearch = true;
    }

    public void ActiveResearchProgressUI(ResearchUnit researchUnit)
    {
        gameObject.SetActive(true);

        SetResearchUnit(researchUnit);

        SetResearchItemImage();
        SetResearchProgressUIText();
        SetResearchMaterialObject();
    }

    public void SetResearchUnit(ResearchUnit researchUnit)
    {
        this.researchUnit = researchUnit;
        this.researchInfo = researchUnit.researchInfo;
    }

    public void SetResearchItemImage()
    {
        researchImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(researchUnit.researchInfo.imgPath);
    }

    public void SetResearchProgressUIText()
    {
        researchItemNameText.GetComponent<Text>().text = researchInfo.name + " " + researchInfo.level.ToString();
        researchExplainText.GetComponent<Text>().text = researchInfo.note;
        levelValueText.GetComponent<Text>().text = researchInfo.level.ToString();
        effectValueText.GetComponent<Text>().text = researchInfo.researchValue.ToString();
    }

    public void SetResearchMaterialObject()
    {
        int requireMaterialListCount = researchUnit.requireMaterialList.Count;
        int requireMaterialIndex = 0;

        foreach (Coordinate researchMaterialCoordinate in researchMaterialCoordinateList[requireMaterialListCount - 1])
        {
            GameObject requireMaterial = requireMaterialSlotList[requireMaterialIndex];
            string requireMaterialText = "";

            requireMaterial.SetActive(true);

            requireMaterial.transform.SetParent(gameObject.transform.Find("RequireMaterialSlots"));
            requireMaterial.GetComponent<RectTransform>().anchoredPosition = new Vector3(researchMaterialCoordinate.getX(), researchMaterialCoordinate.getY(), 0);

            requireMaterial.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ItemManager.Instance.GetInfo(researchUnit.requireMaterialList[requireMaterialIndex].requireMaterialId).imgPath);
            
            int requireMaterialAmount = 0;
            if (ItemManager.Instance.IsDataExist(researchUnit.requireMaterialList[requireMaterialIndex].requireMaterialId))
                requireMaterialAmount = ItemManager.Instance.GetData(researchUnit.requireMaterialList[requireMaterialIndex].requireMaterialId).amount;
            requireMaterialText = requireMaterialAmount.ToString() + "/" + researchUnit.requireMaterialList[requireMaterialIndex].requireMaterialCount.ToString();

            requireMaterial.transform.Find("Text").GetComponent<Text>().text = requireMaterialText;

            if (requireMaterialAmount >= researchUnit.requireMaterialList[requireMaterialIndex].requireMaterialCount)
            {
                requireMaterial.transform.Find("Text").GetComponent<Text>().color = Color.green;
            }

            else
            {
                requireMaterial.transform.Find("Text").GetComponent<Text>().color = Color.red;
                canResearch = false;
            }

            requireMaterialIndex++;
        }
    }

    public void UpdateResearchMaterialItem()
    {
        foreach (ResearchUnit.RequireMaterial requireMaterial in researchUnit.requireMaterialList)
        {
            ItemManager.Instance.RemoveData(requireMaterial.requireMaterialId, requireMaterial.requireMaterialCount);
        }
    }

    public void ResearchProgressUIEvent()
    {
        if (canResearch)
        {
            UpdateResearchMaterialItem();
            UpdateResearchProgressList();

            foreach (GameObject requireMaterialSlot in requireMaterialSlotList)
            {
                requireMaterialSlot.SetActive(false);
            }
        }
    }

    private void UpdateResearchProgressList()
    {
        ResearchManager.Instance.AddData(researchUnit.researchID);

        GameObject[] researchItemUIObjectList = GameObject.FindGameObjectsWithTag("ResearchItem");

        foreach (GameObject researchItemUIObject in researchItemUIObjectList)
        {
            researchItemUIObject.GetComponent<ResearchItemUI>().UpdateResearchItemUI();
        }
    }
}
