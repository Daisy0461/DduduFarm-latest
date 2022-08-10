using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditModes : MonoBehaviour 
{
    [SerializeField] GridBuildingSystem tilemap;
    [SerializeField] DduduSpawner dduduSpawner;
    BuildingManager BM;

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

    private void Start() 
    {
        cam = Camera.main;
        BM = BuildingManager.Instance;
    }

    private void OnEnable() 
    {
        tilemap.gameObject.SetActive(true);
        // TM.UIObjActiveManage(false);
        // TM.enabled = false;

        selectedBuilding.isPointerDown = true;
        selectedBuilding.render.sortingLayerName = "UI";
        selectedBuilding.render.sortingOrder = 0;
        selectedBuilding.GetComponentInChildren<PolygonCollider2D>().isTrigger = true;
    }

    private void OnDisable() 
    {
        selectedBuilding.GetComponentInChildren<PolygonCollider2D>().isTrigger = false;
        selectedBuilding.render.sortingOrder = (int)(selectedBuilding.transform.position.y * -10);
        selectedBuilding.render.sortingLayerName = "Object";
        selectedBuilding.isPointerDown = false;
        
        // TM.enabled = true;
        // TM.UIObjActiveManage(true);
        tilemap.gameObject.SetActive(false);
    }

    private void Update() 
    {
        this.transform.position = cam.WorldToScreenPoint(selectedBuilding.render.transform.position);
    }

    public void OnClickFix()
    {
        if (selectedBuilding.CanBePlaced())
        {
            selectedBuilding.isPointerDown = false;
            // BM.GetData(selectedBuilding.data.id).SetPos(selectedBuilding.transform.position);
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
        if (selectedBuilding.prePos == Vector3.zero)
        {
            // OnClickInventory();
            
            tilemap.CancelBuilding();
            this.gameObject.SetActive(false);
            return;
        }
        selectedBuilding.transform.position = selectedBuilding.prePos;
        this.transform.position = cam.WorldToScreenPoint(selectedBuilding.transform.position);
        selectedBuilding.isPointerDown = false;
        // BM.GetData(selectedBuilding.data.id).SetPos(selectedBuilding.transform.position);
        BM.Save();

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
        PopupBuildingWarning.GetComponentInChildren<Text>().text = "보관 시 생산 중인 아이템이 모두 사라집니다.\n정말 보관하시겠습니까?";
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
        Destroy(selectedBuilding.gameObject);
        BM.Save();
        
        this.gameObject.SetActive(false);
    }

    public void InitPanSell()   // Btn_Sell 에 할당되어 있는 기능
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
        Destroy(selectedBuilding.gameObject);

        this.gameObject.SetActive(false);
    }
}
