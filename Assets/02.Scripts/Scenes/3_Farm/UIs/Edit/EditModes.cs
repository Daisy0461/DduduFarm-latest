using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditModes : MonoBehaviour 
{
    public GridBuildingSystem tilemap;
    [SerializeField] DduduSpawner dduduSpawner;
    BuildingManager BM;
    BuildingAttrib buildingAttrib;

    [Header("EditUI")]
    public EditUI editUI;          
    public TouchManager TM;               

    [Header("Edit Modes")]
    public Building selectedBuilding;     
    public GameObject PopupBuildingWarning;
    public GameObject PopupError;

    [Header("Sell")]
    public GameObject Pan_Sell;     
    public MoneyText moneyText;
    Camera cam;

#region  Unity Method

    private void Start() 
    {
        cam = Camera.main;
        BM = BuildingManager.Instance;
    }

    private void OnEnable() 
    {
        tilemap.gameObject.SetActive(true);
        TM.UIObjActiveManage(false);
        TM.enabled = false;

        buildingAttrib = selectedBuilding.GetComponent<BuildingAttrib>();
        selectedBuilding.isPointerDown = true;
        selectedBuilding.render.sortingLayerName = "UI";
        selectedBuilding.GetComponentInChildren<PolygonCollider2D>().isTrigger = true;
    }

    private void OnDisable() 
    {
        selectedBuilding.GetComponentInChildren<PolygonCollider2D>().isTrigger = false;
        selectedBuilding.render.sortingOrder = (int)((selectedBuilding.transform.position.y + selectedBuilding.render.transform.localPosition.y) * -9 
                    + (selectedBuilding.transform.position.x + selectedBuilding.render.transform.localPosition.x) * -9);
        selectedBuilding.render.sortingLayerName = "Object";
        selectedBuilding.isPointerDown = false;
        
        TM.enabled = true;
        TM.UIObjActiveManage(true);
        tilemap.ClearArea();
        tilemap.gameObject.SetActive(false);
        BM.Save();
    }

    private void Update() 
    {
        this.transform.position = cam.WorldToScreenPoint(selectedBuilding.render.transform.position);
    }

#endregion

    public void OnClickFix()
    {
        if (selectedBuilding.CanBePlaced())
        {
            BM.GetData(buildingAttrib.data.id).SetPos(buildingAttrib.transform.position);
            tilemap.SetBuilding();
            this.gameObject.SetActive(false);
        }
        else
        {
            PopupError.transform.GetChild(0).gameObject.SetActive(true); 
            PopupError.transform.GetChild(1).GetComponent<TextObject>().contentText.text = "설치할 수 없습니다.";
            PopupError.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void OnClickCancel()
    {   
        if (selectedBuilding.prePos == Vector3.zero)    // instantiate cancel
        {
            OnclickInventoryYes();
        }
        else                                            // edit cancel
        {
            selectedBuilding.transform.position = selectedBuilding.prePos;
            BM.GetData(buildingAttrib.data.id).SetPos(selectedBuilding.transform.position);
            tilemap.SetBuilding();
            this.gameObject.SetActive(false);
        }
    }

    public void OnClickInventory()
    {
        if (buildingAttrib.data.info.code >= (int)DataTable.Craft 
            && !selectedBuilding.GetComponent<Craft>().IsWorking())
        {
            OnclickInventoryYes();
        }
        else
        {
            PopupBuildingWarning.GetComponentInChildren<Text>().text = "보관 시 생산 중인 아이템이 모두 사라집니다.\n정말 보관하시겠습니까?";
            PopupBuildingWarning.SetActive(true);
        }
    }

    public void OnclickInventoryYes()
    {// PopupBuildingWarning - yes funciotn
        /* -- working --  */
        if (buildingAttrib.data.info.code >= (int)DataTable.Craft 
            && selectedBuilding.GetComponent<Craft>().IsWorking())
        {
            DduduManager.Instance.GetData(buildingAttrib.data.workerId).isWork = false;
            dduduSpawner.FindDduduObject(buildingAttrib.data.workerId).gameObject.SetActive(true);
            buildingAttrib.data.workerId = 0;
        }
        buildingAttrib.data.isDone = false;
        buildingAttrib.data.cycleRemainTime = 0;
        
        /* -- building eliminate --  */
        buildingAttrib.data.isBuilded = false;
        editUI.CreateEditBtnUI(buildingAttrib.data);
        Destroy(selectedBuilding.gameObject);
        this.gameObject.SetActive(false);
    }

    public void InitPanSell()   // assign -> Btn_Sell
    {
        Pan_Sell.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "{0}.을 \n판매하시겠습니까?".FormatK(buildingAttrib.data.info.name);
        Pan_Sell.transform.GetChild(1).transform.GetChild(2).GetComponent<Text>().text = (buildingAttrib.data.info.sellCost).ToString();
        Pan_Sell.SetActive(true);
    }

    public void OnClickSell()
    {
        BM.RemoveData(buildingAttrib.data);
        ConstructionUI.SpendMoney(-buildingAttrib.data.info.sellCost);
        moneyText.TextUpdate();
        Destroy(selectedBuilding.gameObject);

        this.gameObject.SetActive(false);
    }

    private void OnApplicationFocus(bool focusStatus) 
    {
        buildingAttrib.data = BuildingManager.Instance.GetData(buildingAttrib.data.id);
    }
}
