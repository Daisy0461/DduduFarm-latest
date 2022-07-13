using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditItem : MonoBehaviour
{   // 스크롤뷰 안에 있는 버튼이 가지고 있는 스크립트
    // 주 기능 : 건설 상품을 씬 뷰에 생성하기
    public Text infoText;
    public GameObject[] EditCommonPrefab;
    public GameObject[] EditCraftPrefab;
    public EditUI editUI;
    public BuildingData data;

    public void OnClickEditItem()
    {
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2));
        int type = (0 <= data.info.code && data.info.code <= 50) ? (int)DataTable.Common : (int)DataTable.Craft;
        EditBuilding newItem;
        if (type == (int)DataTable.Common)    // Common
            newItem = Instantiate(EditCommonPrefab[data.info.code-type-1], new Vector3(pos.x, pos.y, 0f), Quaternion.Euler(0f, 0f, 0f)).GetComponent<EditBuilding>();
        else
        {
            newItem = Instantiate(EditCraftPrefab[data.info.code-type-1], new Vector3(pos.x, pos.y, 0f), Quaternion.Euler(0f, 0f, 0f)).GetComponent<EditBuilding>();
            newItem.GetComponent<Craft>().DS = editUI.DS;
        }
        newItem.data = BuildingManager.Instance.GetUnBuildedData(data.info.code);
        newItem.editModesObject = editUI.editModes;
        newItem.popupBuilding = editUI.popupBuildings[type== (int)DataTable.Common ? 0 : 1];
        newItem.transform.parent = editUI.parentBuildings.transform;

        newItem.isPointerDown = true;
        newItem.editModes = editUI.editModes.GetComponent<EditModes>();
        newItem.EditModeActive();
        newItem.data.isBuilded = true;
        
        if (editUI.BM.GetBuildingAmount(data.info.code) - editUI.BM.GetBuildedAmount(data.info.code) > 0)
            infoText.text = (editUI.BM.GetBuildingAmount(data.info.code) - editUI.BM.GetBuildedAmount(data.info.code)) + " 개";
        else Destroy(this.gameObject);
    }
}
