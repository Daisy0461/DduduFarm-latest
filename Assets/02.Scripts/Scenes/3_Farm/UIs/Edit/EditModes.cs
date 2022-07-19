using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditModes : MonoBehaviour, IDragHandler
{
    [SerializeField] DduduSpawner dduduSpawner;
    BuildingManager BM;

    [Header("EditUI")]
    public EditUI editUI;
    public Button editQuit;             // ?���? 모드 ?��?��?�� ?�� ?���? ?��벤토�? ?���?
    public TouchManager TM;               // ?���? 모드 ?��?��?�� ?�� UI ?��?���? 비활?��?���? ?���? 모드?�� 주목

    [Header("Edit Modes")]
    public GameObject selectedBuilding; // ?��?�� ?��?��?�� 건물
    public EditBuilding selectedEditBuilding;
    public SpriteRenderer selectedBuildingRenderer;
    public EditBuildingGrid selectedBuildingGrid;
    public Button editModesQuit;        // ?���? ?�� ?���??���? ?��집모?�� 종료
    public GameObject PopupBuildingWarning;

    [Header("Fixable")]
    public GameObject btnBlock;         // 배치?�� ?�� ?��?�� 곳이�? ?��?���? 조절
    public bool isAbleToFix = true;     // 배치?�� ?�� ?��?���? ?��?���??

    [Header("Sell")]
    public GameObject Pan_Sell;         // ?���? 버튼 ?���? ?�� ?��?��?�� ?��?�� ?��?��
    public MoneyText moneyText;

    private void Start() 
    {
        BM = BuildingManager.Instance;
    }

    private void OnEnable() 
    {
        editQuit.onClick.Invoke();  // ?��?��?�� ?��?�� 버튼?�� ?��?��립트�? ?���??���?
        editModesQuit.gameObject.SetActive(true);
        TM.UIObjActiveManage(false);
        TM.scrollable = false;

        selectedBuildingRenderer.sortingOrder = 0;
        selectedBuilding.GetComponent<Collider2D>().isTrigger = true;
        // selectedBuildingGrid.GetComponent<Collider2D>().isTrigger = true;
        selectedBuildingGrid.spriteRenderer.enabled = true;
    }

    private void OnDisable() 
    {
        selectedBuilding.GetComponent<Collider2D>().isTrigger = false;
        // selectedBuildingGrid.GetComponent<Collider2D>().isTrigger = false;
        selectedBuildingGrid.spriteRenderer.enabled = false;
        
        selectedEditBuilding.isPointerDown = false;
        selectedBuilding = null;
        TM.UIObjActiveManage(true);
        TM.scrollable = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!selectedEditBuilding.isPointerDown)
            return;
        // ?��?�� 모션
        selectedBuilding.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = Input.mousePosition;
        selectedBuilding.transform.position -= new Vector3(0,0,-10);
        
    }

    public void OnClickFix()
    {
        if (isAbleToFix)
        {
            selectedEditBuilding.isPointerDown = false;
            BM.GetData(selectedEditBuilding.data.id).SetPos(selectedBuilding.transform.position);
            selectedBuildingRenderer.sortingOrder = (int)(selectedBuilding.transform.position.y * -10);
            BM.Save();
            this.gameObject.SetActive(false);
            editModesQuit.gameObject.SetActive(false);
        }
        else
            Debug.Log("배치?�� ?�� ?��?��?��?��.");
    }

    public void OnClickCancel()
    {   // ?��?�� ?�� ?��치로 ?��?��리기
        if (selectedEditBuilding.prePos == new Vector3(0,0,0))
        {
            OnClickInventory();
            return;
        }
        selectedBuilding.transform.position = selectedEditBuilding.prePos;
        this.transform.position = Camera.main.WorldToScreenPoint(selectedBuilding.transform.position);
        selectedEditBuilding.isPointerDown = false;
        BM.GetData(selectedEditBuilding.data.id).SetPos(selectedBuilding.transform.position);
        selectedBuildingRenderer.sortingOrder = (int)(selectedBuilding.transform.position.y * -10);
        BM.Save();

        editModesQuit.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void OnClickInventory()
    {   // ?��?��중인 건물?���? ?��?���? ?��?���?(?��반이�? 무조�?, 공방?���? 그냥 보�??)
        if (selectedEditBuilding.craft != null)
        {
            if (!selectedEditBuilding.craft.IsWorking())
            {
                OnclickInventoryYes();
                return;
            }
        }
        PopupBuildingWarning.SetActive(true);
    }

    public void OnclickInventoryYes()
    {
        /* -- working --  */
        if (selectedEditBuilding.craft != null && selectedEditBuilding.craft.IsWorking())
        {
            // ?��?�� ?��?�� ?��리기
            selectedEditBuilding.craft.data.worker.isWork = false;
            dduduSpawner.FindDduduObject(selectedEditBuilding.craft.data.worker.id).gameObject.SetActive(true);
            // worker ?���?
            selectedEditBuilding.craft.data.worker = null;
        }
        selectedEditBuilding.data.isDone = false;
        selectedEditBuilding.data.cycleRemainTime = 0;
        
        /* -- building eliminate --  */
        BM.GetData(selectedEditBuilding.data.id).isBuilded = false;
        editUI.CreateEditBtnUI(BM.GetData(selectedEditBuilding.data.id));
        Destroy(selectedBuilding);
        BM.Save();
        
        editModesQuit.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void InitPanSell()
    {
        BuildingData data = selectedEditBuilding.data;
    
        Pan_Sell.GetComponentInChildren<Text>().text = data.info.name + "�? \n?��매하?��겠습?���??";
        Pan_Sell.transform.GetChild(2).GetComponent<Text>().text = (data.info.sellCost).ToString();
    }

    public void OnClickSell()
    {   // ?���?
        BuildingData data = BM.GetData(selectedEditBuilding.data.id);
        BM.RemoveData(data);
        // ?��매로 ?��?�� ?��?��
        ConstructionUI.SpendMoney(-data.info.sellCost);
        moneyText.TextUpdate();
        
        Destroy(selectedBuilding);
        editModesQuit.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
