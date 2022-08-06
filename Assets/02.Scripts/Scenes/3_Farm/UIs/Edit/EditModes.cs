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
    public GameObject selectedBuildingObject; 
    public Building selectedBuilding;
    public SpriteRenderer selectedBuildingRenderer;
    public Button editModesQuit;        
    public GameObject PopupBuildingWarning;

    [Header("Fixable")]     
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
        selectedBuildingObject.GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnDisable() 
    {
        selectedBuildingObject.GetComponent<Collider2D>().isTrigger = false;
        
        selectedBuilding.isPointerDown = false;
        selectedBuildingObject = null;
        TM.UIObjActiveManage(true);
        TM.scrollable = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!selectedBuilding.isPointerDown)
            return;
        
        selectedBuildingObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = Input.mousePosition;
        selectedBuildingObject.transform.position -= new Vector3(0,0,-10);
    }

    public void OnClickFix()
    {
        if (isAbleToFix)
        {
            selectedBuilding.isPointerDown = false;
            // BM.GetData(selectedBuilding.data.id).SetPos(selectedBuildingObject.transform.position);
            selectedBuildingRenderer.sortingOrder = (int)(selectedBuildingObject.transform.position.y * -10);
            BM.Save();
            this.gameObject.SetActive(false);
            editModesQuit.gameObject.SetActive(false);
        }
        else
            Debug.Log("didn't installed");
    }

    public void OnClickCancel()
    {   
        if (selectedBuilding.prePos == new Vector3(0,0,0))
        {
            OnClickInventory();
            return;
        }
        selectedBuildingObject.transform.position = selectedBuilding.prePos;
        this.transform.position = Camera.main.WorldToScreenPoint(selectedBuildingObject.transform.position);
        selectedBuilding.isPointerDown = false;
        // BM.GetData(selectedBuilding.data.id).SetPos(selectedBuildingObject.transform.position);
        selectedBuildingRenderer.sortingOrder = (int)(selectedBuildingObject.transform.position.y * -10);
        BM.Save();

        editModesQuit.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void OnClickInventory()
    {
        if (selectedBuilding.GetComponent<Craft>() != null)
        {
            if (!selectedBuilding.GetComponent<Craft>().IsWorking())
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
        if (selectedBuilding.GetComponent<Craft>() != null && selectedBuilding.GetComponent<Craft>().IsWorking())
        {
            DduduManager.Instance.GetData(selectedBuilding.GetComponent<Craft>().data.workerId).isWork = false;
            dduduSpawner.FindDduduObject(selectedBuilding.GetComponent<Craft>().data.workerId).gameObject.SetActive(true);
            selectedBuilding.GetComponent<Craft>().data.workerId = 0;
        }
        // selectedBuilding.data.isDone = false;
        // selectedBuilding.data.cycleRemainTime = 0;
        
        /* -- building eliminate --  */
        // BM.GetData(selectedBuilding.data.id).isBuilded = false;
        // editUI.CreateEditBtnUI(BM.GetData(selectedBuilding.data.id));
        Destroy(selectedBuildingObject);
        BM.Save();
        
        editModesQuit.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void InitPanSell()
    {
        // BuildingData data = selectedBuilding.data;
    
        // Pan_Sell.GetComponentInChildren<Text>().text = data.info.name + "을 \n판매하시겠습니까?";
        // Pan_Sell.transform.GetChild(2).GetComponent<Text>().text = (data.info.sellCost).ToString();
    }

    public void OnClickSell()
    {
        // BuildingData data = BM.GetData(selectedBuilding.data.id);
        // BM.RemoveData(data);
        // ConstructionUI.SpendMoney(-data.info.sellCost);
        moneyText.TextUpdate();
        
        Destroy(selectedBuildingObject);
        editModesQuit.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
