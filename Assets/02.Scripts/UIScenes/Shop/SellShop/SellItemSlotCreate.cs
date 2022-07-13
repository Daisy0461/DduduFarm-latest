using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellItemSlotCreate : MonoBehaviour
{
    [HideInInspector] public ItemManager IM;
    [HideInInspector] public BuildingManager BM;
    [HideInInspector] public CropManager CM;
    [HideInInspector] public FishManager FM;

    [SerializeField] private SellCount SC;
    public Text moneyText;
    public GameObject SellItemPrefab;
    public GameObject parentContent;
    public List<GameObject> slots = new List<GameObject>();
    public SellSlot selectedSlot;
    public AudioSource audioSource;

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

    [Header("Info Popup - Common")]         
    public GameObject popupCommon;          
    public Text common_nameTxt;            
    public Image common_iconImg;            
    public Text common_explainTxt;          
    public Text common_goldCycleTxt;               
    public Text common_goldOutputTxt;              

    [Header("Info Popup - Craft")]
    public GameObject popupCraft;                   
    public Text craft_nameText;                 
    public Image craft_iconImg;                 
    public Text craft_explainTxt;                  
    public Text craft_CycleTxt;                  
    public Text craft_satietyTxt;                 
    public Image craft_materialImg;                   
    public Text craft_materialTxt;                    
    public Image craft_outputImg;                 
    public Text craft_outputTxt; 

    private void Awake()                            // ShopScene에 들어왔을 때 호출
    {  
        IM = ItemManager.Instance;
        BM = BuildingManager.Instance;
        CM = CropManager.Instance;
        FM = FishManager.Instance;
    }

    private void OnEnable() 
    {
        SC.OnClickNo();   
        MoneyText.ChangeMoneyText(moneyText, IM.GetData((int)DataTable.Money).amount);
        
        while (slots.Count < IM.GetDataListCount(true) + BM.GetUniqueUnBuildedAmount())    // 현재 가진 잡화, 건물의 개수 만큼 slot 만들기 - 판매 시 slot 제거
            SlotMaker();
        while (slots.Count > IM.GetDataListCount(true) + BM.GetUniqueUnBuildedAmount())
        {
            var tmp = slots[slots.Count-1];
            PopSlot();
            Destroy(tmp);
        }  
        SellItemCreate();
    }

    public void PopSlot()
    {
        slots.RemoveAt(index: slots.Count-1);
    }

    public void PopSlot(GameObject tmp)
    {
        slots.Remove(tmp);
    }

    public void SlotMaker()
    {
        var newObj = Instantiate(SellItemPrefab, parentContent.transform);
        slots.Add(newObj);
    }    

    public void SellItemCreate()
    {
        int index = 0;
        foreach (var data in IM.GetDataList())
        {
            if (data.id == (int)DataTable.Love || data.id == (int)DataTable.Money || data.info.sellCost <= 0)
                continue;
            var newObj = slots[index].GetComponent<SellSlot>();
            newObj.iData = data;
            newObj.bCode = 0;
            SetItem(newObj, data, null);

            index++;
        }
        foreach (var data in BM.GetUniqueUnBuildedBuilding())
        {
            var newObj = slots[index].GetComponent<SellSlot>();
            newObj.bCode = data.info.code; 
            SetItem(newObj, null, data);
            
            index++;
        }
    }

    void SetItem(SellSlot newObj, ItemData iData, BuildingData bData)
    {
        newObj.SI = this;
        Image newImg = newObj.transform.GetChild(0).GetComponent<Image>();
        Text amtText = newObj.transform.GetChild(0).GetComponentInChildren<Text>();
        Text[] newText = newObj.transform.GetChild(1).GetComponentsInChildren<Text>();

        if (iData != null) {
            newImg.sprite = Resources.Load<Sprite>(iData.info.imgPath);
            amtText.text = iData.amount.ToString();
            newText[0].text = iData.info.name;
            if (iData.info.sellCost == 0)
                newText[1].text = string.Format("{0:#,##0}", iData.info.buyCost/5);
            else
                newText[1].text = string.Format("{0:#,##0}", (iData.info.sellCost));
        } else {
            newImg.sprite = Resources.Load<Sprite>(bData.info.imgPath);
            amtText.text = BM.GetBuildingAmount(bData.info.code).ToString();
            newText[0].text = bData.info.name;
            newText[1].text = string.Format("{0:#,##0}", bData.info.sellCost);
        }
        if (!newObj.gameObject.activeSelf)
            newObj.gameObject.SetActive(true);
    }
}
