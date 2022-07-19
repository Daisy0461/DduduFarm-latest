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
    public Button editQuit;             // ?¸ì§? ëª¨ë“œ ?™œ?„±?™” ?‹œ ?¸ì§? ?¸ë²¤í† ë¦? ?‹«ê¸?
    public TouchManager TM;               // ?¸ì§? ëª¨ë“œ ?™œ?„±?™” ?‹œ UI ?•„?´ì½? ë¹„í™œ?„±?™”ë¡? ?¸ì§? ëª¨ë“œ?— ì£¼ëª©

    [Header("Edit Modes")]
    public GameObject selectedBuilding; // ?˜„?¬ ?„ ?ƒ?œ ê±´ë¬¼
    public EditBuilding selectedEditBuilding;
    public SpriteRenderer selectedBuildingRenderer;
    public EditBuildingGrid selectedBuildingGrid;
    public Button editModesQuit;        // ?‹¤ë¥? ?° ?´ë¦??•˜ë©? ?¸ì§‘ëª¨?“œ ì¢…ë£Œ
    public GameObject PopupBuildingWarning;

    [Header("Fixable")]
    public GameObject btnBlock;         // ë°°ì¹˜?•  ?ˆ˜ ?—†?Š” ê³³ì´ë©? ?•Œ?ŒŒê°? ì¡°ì ˆ
    public bool isAbleToFix = true;     // ë°°ì¹˜?•  ?ˆ˜ ?ˆ?Š”ê°? ?—†?Š”ê°??

    [Header("Sell")]
    public GameObject Pan_Sell;         // ?Œë§? ë²„íŠ¼ ?´ë¦? ?‹œ ?™œ?„±?™” ?˜?Š” ?Œ¨?„
    public MoneyText moneyText;

    private void Start() 
    {
        BM = BuildingManager.Instance;
    }

    private void OnEnable() 
    {
        editQuit.onClick.Invoke();  // ?—?””?„° ?ƒ?˜ ë²„íŠ¼?„ ?Š¤?¬ë¦½íŠ¸ë¡? ?´ë¦??•˜ê¸?
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
        // ?´?™ ëª¨ì…˜
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
            Debug.Log("ë°°ì¹˜?•  ?ˆ˜ ?—†?Šµ?‹ˆ?‹¤.");
    }

    public void OnClickCancel()
    {   // ?´?™ ? „ ?œ„ì¹˜ë¡œ ?˜?Œë¦¬ê¸°
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
    {   // ?‘?—…ì¤‘ì¸ ê±´ë¬¼?´ë©? ?•ˆ?‚´ì°? ?„?š°ê¸?(?¼ë°˜ì´ë©? ë¬´ì¡°ê±?, ê³µë°©?´ë©? ê·¸ëƒ¥ ë³´ê??)
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
            // ?šœ?‘ ?”¬?— ?˜¬ë¦¬ê¸°
            selectedEditBuilding.craft.data.worker.isWork = false;
            dduduSpawner.FindDduduObject(selectedEditBuilding.craft.data.worker.id).gameObject.SetActive(true);
            // worker ? œê±?
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
    
        Pan_Sell.GetComponentInChildren<Text>().text = data.info.name + "ë¥? \n?Œë§¤í•˜?‹œê² ìŠµ?‹ˆê¹??";
        Pan_Sell.transform.GetChild(2).GetComponent<Text>().text = (data.info.sellCost).ToString();
    }

    public void OnClickSell()
    {   // ?Œë§?
        BuildingData data = BM.GetData(selectedEditBuilding.data.id);
        BM.RemoveData(data);
        // ?Œë§¤ë¡œ ?¸?•œ ?ˆ˜?µ
        ConstructionUI.SpendMoney(-data.info.sellCost);
        moneyText.TextUpdate();
        
        Destroy(selectedBuilding);
        editModesQuit.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
