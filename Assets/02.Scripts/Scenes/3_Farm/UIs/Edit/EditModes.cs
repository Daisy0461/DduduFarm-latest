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
    public Button editQuit;             
    public TouchManager TM;               

    [Header("Edit Modes")]
    public GameObject selectedBuilding; 
    public EditBuilding selectedEditBuilding;
    public SpriteRenderer selectedBuildingRenderer;
    public EditBuildingGrid selectedBuildingGrid;
    public Button editModesQuit;        
    public GameObject PopupBuildingWarning;

    [Header("Fixable")]
    public GameObject btnBlock;         
    public bool isAbleToFix = true;   

    [Header("Sell")]
    public GameObject Pan_Sell;     
    public MoneyText moneyText;

    private void Start() 
    {
        BM = BuildingManager.Instance;
    }

    private void OnEnable() 
    {
        editQuit.onClick.Invoke();  
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
            Debug.Log("didn't installed");
    }

    public void OnClickCancel()
    {   
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
    {
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
            DduduManager.Instance.GetData(selectedEditBuilding.craft.data.workerId).isWork = false;
            dduduSpawner.FindDduduObject(selectedEditBuilding.craft.data.workerId).gameObject.SetActive(true);
            selectedEditBuilding.craft.data.workerId = 0;
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
    
        Pan_Sell.GetComponentInChildren<Text>().text = data.info.name + "을 \n판매하시겠습니까?";
        Pan_Sell.transform.GetChild(2).GetComponent<Text>().text = (data.info.sellCost).ToString();
    }

    public void OnClickSell()
    {
        BuildingData data = BM.GetData(selectedEditBuilding.data.id);
        BM.RemoveData(data);
        ConstructionUI.SpendMoney(-data.info.sellCost);
        moneyText.TextUpdate();
        
        Destroy(selectedBuilding);
        editModesQuit.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
