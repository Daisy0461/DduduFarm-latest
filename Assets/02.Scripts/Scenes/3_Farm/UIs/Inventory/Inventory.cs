using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Inventory : MonoBehaviour
{
    ItemManager IM;
    CropManager CM;
    FishManager FM;

    public GameObject ItemPrefab;
    public GameObject ItemPrefabParent;
    public List<GameObject> slots = new List<GameObject>();

    [Header("Extension")]
    public Text SlotText;

    [Header("DownButtons")]
    public Image buttonImage;
    public GameObject Pan_TrashBG;
    bool isTrashMode = false;
    List<GameObject> trashList = new List<GameObject>();
    public Sprite trashSelectedBG;
    public Sprite trashOriginBG;

    public string sortStatus = "Obtain";
    
    int count;

    enum showType {
        All = 0,
        Farm = 2,
        FishFarm = 4,
        Output = 3,
        Gem = 7
    }
    showType curShowType = 0;

    private void Awake()
    {
        IM = ItemManager.Instance;
        CM = CropManager.Instance;
        FM = FishManager.Instance;

        for (int i = 0; i < IM.maxDataListValue; i++)
            SlotMaker();
    }

    private void OnEnable()
    {
        curShowType = 0;
        OnClickColorChange(null);
        Show();
    }

    public void SlotMaker() // 인벤토리 슬롯 개설
    {
        var newObj = Instantiate(ItemPrefab, ItemPrefabParent.transform);
        newObj.GetComponentInChildren<Button>().onClick.AddListener(() => OnClickItem(newObj));
        slots.Add(newObj);
    }

    void OnClickItem(GameObject slot)   // 아이템 클릭 시 정보 패널 오픈, 버리기 모드 시 버릴 리스트에 적립
    {
        //아이템 클릭 시
        if (slot.GetComponentInChildren<InventoryItemInfo>().code == 0) return;
        if (isTrashMode)
        {
            if (slot.GetComponent<Image>().sprite == trashSelectedBG) 
            {
                trashList.Remove(slot);
                slot.GetComponent<Image>().sprite = trashOriginBG;
                return;
            } 
            else 
            {
                trashList.Add(slot);
                slot.GetComponent<Image>().sprite = trashSelectedBG;
                return;
            }
        } 
        SelectPopup(slot);
    }

    public void Show()  // 인벤토리를 열 때마다 인벤토리 내의 아이템을 보여주는 메서드(정렬 / 슬롯에 아이템 정보 삽입)
    {
        count = IM.GetDataListCount();
        SlotText.text = "보유슬롯 " + count.ToString() + "/" + IM.maxDataListValue.ToString();
        if (count <= 0) return;

        if (sortStatus == "Latest")
            SortLatestFirst();
        else if (sortStatus == "Obtain")
            SortObtainFirst();

        PrintShow();
    }

    public void PrintShow()
    {
        int i = 0;
        foreach (var data in IM.GetDataList())
        {
            if (data.id == (int)DataTable.Money || data.id == (int)DataTable.Love) 
                continue;
            if (slots.Count < i-1) 
            {
                Debug.Log("인벤토리 제한 보다 많습니다.");
                return;
            }
            // 전체 혹은 showType에 맞으면 출력
            if (curShowType == 0 || curShowType == (showType)(data.id/100))
            {
                slots[i].GetComponentInChildren<InventoryItemInfo>().code = data.id;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(data.info.imgPath);
                slots[i].GetComponentInChildren<Text>().text = data.amount.ToString();
                i++;
            }
        }
    }

    void ClearShow() // 모드 변경(전체 보기에서 보석보기로 변경 등)시 슬롯 보여지는 것 지우기
    {
        for (int i=0; i<IM.maxDataListValue; i++)   // 보여지거나 검사되는 것만 초기화 시킨다
        {
            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = trashOriginBG;
            slots[i].GetComponentInChildren<Text>().text = null;
        }
    }

// 정렬 방식
    public void SortLatestFirst()
    {
        // 최신순 : 가장 최근에 획득한 것이 상위에 노출
        IM.GetDataList().Sort( 
            delegate(ItemData a, ItemData b) { 
                return a.obtainDate.CompareTo(b.obtainDate); 
            }
        );
    }

    public void SortObtainFirst()
    {
        // 획득순 : 가장 처음 획득한 것이 제일 위에 노출
        IM.GetDataList().Sort( 
            delegate(ItemData a, ItemData b) { 
                return b.obtainDate.CompareTo(a.obtainDate); 
            }
        );
    }

// 전체: 0 | 농사: 2 | 양식: 4 | 가공물: 3 | 보석: 7
    public void OnClickTypeChange(int type)
    {
        if (curShowType == (showType)type) 
            curShowType = showType.All;
        else 
            curShowType = (showType)type;
        ClearShow();
        PrintShow();
    }

    public void OnClickColorChange(Image img)
    {
        if (buttonImage != null)
        {
            buttonImage.color = Color.white;
            if (buttonImage == img) 
            {
                buttonImage = null;
                return;
            }
            buttonImage = null;
        }
        if (img == null) return;
        if (buttonImage == null)
            buttonImage = img;
        buttonImage.color = Color.yellow;
    }

    public void OnClickTrashToggle(Text text)
    {
        if (isTrashMode == false)           // 버리기 모드로 전환
        {
            isTrashMode = true;
            text.text = "선택중";
        }
        else if (isTrashMode == true)       // 버리기 모드 해제 + 선택된 아이템 버리기
        {
            isTrashMode = false;
            text.text = "버리기";
            if (trashList.Count > 0)
            {
                Pan_TrashBG.SetActive(true);
            }
        }
    }

    public void OnClickTrashBtn(bool isYes)
    {
        if (isYes == true)  // 버린다!
        {
            int count = trashList.Count;
            for(int i=count-1; i>=0; i--)
            {
                IM.RemoveData(trashList[i].GetComponentInChildren<InventoryItemInfo>().code, 
                                    IM.GetData(trashList[i].GetComponentInChildren<InventoryItemInfo>().code).amount); 
                trashList[i].GetComponent<Image>().sprite = trashOriginBG;
                trashList.Remove(trashList[i]);   
            }
            count = IM.GetDataListCount();
            SlotText.text = "보유슬롯 " + count.ToString() + "/" + IM.maxDataListValue.ToString();
            OnClickTypeChange((int)curShowType);
        }
        else                // 안 버려! 
        {
            int count = trashList.Count;
            for (int i=0; i<count; i++)
            {
                trashList[0].GetComponent<Image>().sprite = trashOriginBG;
                trashList.Remove(trashList[0]);
            }
        }
    }
}
