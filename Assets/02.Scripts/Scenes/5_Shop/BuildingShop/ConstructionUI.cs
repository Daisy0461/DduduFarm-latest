using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionUI : MonoBehaviour
{
    // 구매하기 버튼
    // 선택한 상품이 있다면 구매 가능
    // 상품 버튼 생성하기
    [Header("instantiate")]
    public GameObject[] CSItemPanelPrefab;                // 건설 상품 프리팹
    public GameObject parentContent;                    // 건설 스크롤뷰 content

    [Header("setup")]
    public Building selectedBuilding;
    public GameObject buyCheckUI;
    public GameObject refuseUI;
    public Text moneyText;
    public TalkTextScript talkTextScript;

    public AudioSource audioSource;

    [HideInInspector] public BuildingManager BM;
    [HideInInspector] public ItemManager IM;
    private List<GameObject> slots = new List<GameObject>();

    [Header("Info Popup - Common")]         
    public GameObject popupCommon;          
    public Text common_nameText;            
    public Image common_iconImg;            
    public Text common_explainTxt;          
    public Text goldCycleTxt;               
    public Text goldOutputTxt;              

    [Header("Info Popup - Craft")]
    public GameObject popupCraft;                   
    public Text craft_nameText;                 
    public Image craft_iconImg;                 
    public Text craft_explainTxt;                  
    public Text craftCycleTxt;                  
    public Text satietyTxt;                 
    public Image materialImg;                   
    public Text materialTxt;                    
    public Image outputImg;                 
    public Text outputTxt;                  
    
    private void Awake() 
    {
        BM = BuildingManager.Instance;
        IM = ItemManager.Instance;
        
        ConstructionCreate();   // 상품 패널 생성
    }

    private void OnEnable() 
    {
        OnClickBuyNo();
        ManageInterlock();
        if (moneyText != null)
                MoneyText.ChangeMoneyText(moneyText, IM.GetData((int)DataTable.Money).amount);
    }

    public void ConstructionCreate()    // 상품 패널 생성
    {   
        foreach (var code in BM.InfoKeys())
        {
            int type = (code < (int)DataTable.Craft) ? 0 : 1;
            var newObj = Instantiate(CSItemPanelPrefab[type], parentContent.transform);

            var newItem = BM.GetInfo(code);
            newObj.GetComponent<Building>().CSUI = this;
            newObj.GetComponent<Building>().info = newItem;

            // 이미지 및 아이템 정보 값 세팅
            Image newImg = newObj.transform.GetChild(0).GetComponent<Image>();
            newImg.sprite = Resources.Load<Sprite>(newItem.imgPath);
            Image icon = newObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();

            Text[] newText = newObj.GetComponentsInChildren<Text>();
            newText[0].text = newItem.name;
            newText[1].text = string.Format("{0:#,##0}", newItem.buyCost);
            
            newObj.GetComponent<Button>().onClick.AddListener(() => OnclickCheckPurchasable());
            slots.Add(newObj);
        }
    }

    int CheckPurchasable(GameObject newObj)    // 패널 선택 시 구매 가능한지 체크 후 구매 불가 안내 대화(상점 주인) 출력 
    {
        int res = 0;
        BuildingInfo newItem = newObj.GetComponent<Building>().info;
        
        // if () 
        // {
        //     res= 3;
        // }
        if ((newItem.code < (int)DataTable.Craft && BM.GetBuildingAmount(newItem.code) >= 3) ||
	            (newItem.code >= (int)DataTable.Craft && BM.GetBuildingAmount(newItem.code) >= 3))
        {
            res= 2;
        }
        else if (newItem.buyCost > IM.GetData((int)DataTable.Money).amount)
        {
            res= 1;
        }
        return res;
    }

    public void OnclickCheckPurchasable()
    {
        if (selectedBuilding == null) return;
        BuildingInfo info = selectedBuilding.info;
        
        // if () 
        // {
        //     talkTextScript.UnableToBuyText("그 건물에 대한 연구가 진행되지 않았다네");
        // }
        if ((info.code < (int)DataTable.Craft && BM.GetBuildingAmount(info.code) >= 3) ||
	            (info.code >= (int)DataTable.Craft && BM.GetBuildingAmount(info.code) >= 3))
        {
            talkTextScript.UnableToBuyText("그 건물은 더 이상 가질 수 없다네");
        }
        else if (info.buyCost > IM.GetData((int)DataTable.Money).amount)
        {
            talkTextScript.UnableToBuyText("그걸 구매하기엔 돈이 모자르군");
        }
        else
        {
            talkTextScript.GreetText();
        }
    }

    int csCnt;
    void ManageInterlock()  // 연쇄관리 - 팔리지 않은 상품은 활성화, 팔린 상품은 비활성화 // 판매 상점에 대한 건설 상점 비실시간 업데이트
    {
        csCnt = 0;
        for (int i=0; i < slots.Count; i++)
        {
            if (CheckPurchasable(slots[i]) == 0)  // 구매할 수 있는지 수 세기
                csCnt++;
        }

        if (csCnt <= 0) talkTextScript.UnableToBuyText("지금은 구매할 수 있는 건물이 없다네");
    }

    public void OnClickBuyThis()    // 구매하기/나가기 버튼 중 구매하기 버튼을 눌렀을 때
    {
        if (selectedBuilding == null)
            return;
        selectedBuilding.GetComponent<Image>().color = Color.white;

        switch (CheckPurchasable(selectedBuilding.gameObject))
        {
            case 0:
                buyCheckUI.SetActive(true);
                break;
            case 1: // 돈이 없어
                refuseUI.GetComponentInChildren<Text>().text = "돈이 부족합니다.";
                refuseUI.SetActive(true);
                talkTextScript.BuyText(true);
                selectedBuilding = null;
                break;
            case 2: // 구매가능 수량이 안돼
                refuseUI.GetComponentInChildren<Text>().text = "더 이상 구매할 수 없습니다.";
                refuseUI.SetActive(true);
                selectedBuilding = null;
                break;
            case 3: // 연구가 안됐어
                refuseUI.GetComponentInChildren<Text>().text = "연구가 필요합니다.";
                refuseUI.SetActive(true);
                selectedBuilding = null;
                break;
        }
    }

    public void OnClickBuy()    // 최종 구매 결정, BuyCheckUI
    {       
        // 돈 소비 함수 호출
        if (SpendMoney(selectedBuilding.info.buyCost))
        {
            MoneyText.ChangeMoneyText(moneyText, IM.GetData((int)DataTable.Money).amount);
            
            csCnt--;
            if (csCnt <= 0)
                talkTextScript.UnableToBuyText("지금은 구매할 수 있는 건물이 없다네");
            else
                talkTextScript.BuyText();
            
            BM.AddData(selectedBuilding.info.code);
            CheckPurchasable(selectedBuilding.gameObject);
        }
        else 
        {
            talkTextScript.BuyText(true);
        }
        selectedBuilding = null;
        buyCheckUI.SetActive(false);
    }

    public void OnClickBuyNo()
    {
        if (selectedBuilding != null)
        {
            selectedBuilding.GetComponent<Image>().color = Color.white;
        }
        selectedBuilding = null;
    }

    static public bool SpendMoney(int use)
    {
        ItemManager IM = ItemManager.Instance;
        int getMoney = IM.GetData((int)DataTable.Money).amount;
        if (getMoney < use) {
            return false;
        } else {
            IM.RemoveData((int)DataTable.Money, use);
            // EncryptedPlayerPrefs.SetInt("Money", (getMoney - use));
            return true;
        }
    }
}
