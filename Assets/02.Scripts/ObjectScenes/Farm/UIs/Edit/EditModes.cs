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
    public Button editQuit;             // 편집 모드 활성화 시 편집 인벤토리 닫기
    public TouchManager TM;               // 편집 모드 활성화 시 UI 아이콘 비활성화로 편집 모드에 주목

    [Header("Edit Modes")]
    public GameObject selectedBuilding; // 현재 선택된 건물
    public EditBuilding selectedEditBuilding;
    public SpriteRenderer selectedBuildingRenderer;
    public EditBuildingGrid selectedBuildingGrid;
    public Button editModesQuit;        // 다른 데 클릭하면 편집모드 종료
    public GameObject PopupBuildingWarning;

    [Header("Fixable")]
    public GameObject btnBlock;         // 배치할 수 없는 곳이면 알파값 조절
    public bool isAbleToFix = true;     // 배치할 수 있는가 없는가?

    [Header("Sell")]
    public GameObject Pan_Sell;         // 판매 버튼 클릭 시 활성화 되는 패널
    public MoneyText moneyText;

    private void Start() 
    {
        BM = BuildingManager.Instance;
    }

    private void OnEnable() 
    {
        editQuit.onClick.Invoke();  // 에디터 상의 버튼을 스크립트로 클릭하기
        editModesQuit.gameObject.SetActive(true);
        TM.UIObjActiveManage(false);
        TM.scrollable = false;

        selectedBuildingRenderer.sortingOrder = 0;
        selectedBuildingGrid.GetComponent<Collider2D>().isTrigger = true;
        selectedBuildingGrid.spriteRenderer.enabled = true;
    }

    private void OnDisable() 
    {
        selectedBuildingGrid.GetComponent<Collider2D>().isTrigger = false;
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
        // 이동 모션
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
            Debug.Log("배치할 수 없습니다.");
    }

    public void OnClickCancel()
    {   // 이동 전 위치로 되돌리기
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
    {   // 작업중인 건물이면 안내창 띄우기(일반이면 무조건, 공방이면 그냥 보관)
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
            // 뚜두 씬에 올리기
            selectedEditBuilding.craft.data.worker.isWork = false;
            dduduSpawner.FindDduduObject(selectedEditBuilding.craft.data.worker.id).gameObject.SetActive(true);
            // worker 제거
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
    
        Pan_Sell.GetComponentInChildren<Text>().text = data.info.name + "를 \n판매하시겠습니까?";
        Pan_Sell.transform.GetChild(2).GetComponent<Text>().text = (data.info.sellCost).ToString();
    }

    public void OnClickSell()
    {   // 판매
        BuildingData data = BM.GetData(selectedEditBuilding.data.id);
        BM.RemoveData(data);
        // 판매로 인한 수익
        ConstructionUI.SpendMoney(-data.info.sellCost);
        moneyText.TextUpdate();
        
        Destroy(selectedBuilding);
        editModesQuit.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
