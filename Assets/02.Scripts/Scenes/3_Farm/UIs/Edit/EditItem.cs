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
        int code = data.info.code;
        Building building;
        
        if (code <= 50)    // Common
        {
            building = editUI.tilemap.InitializeWithBuilding(EditCommonPrefab[code%(int)DataTable.Craft-1]);
            Common common = building.GetComponent<Common>();
            common.data = data;
            common.popupBuilding = editUI.popupBuildings[0];
        } else // Craft
        {
            building = editUI.tilemap.InitializeWithBuilding(EditCraftPrefab[code%(int)DataTable.Craft-1]);
            Craft craft = building.GetComponent<Craft>();
            craft.DS = editUI.DS;
            craft.data = data;
            craft.popupBuilding = editUI.popupBuildings[1];
        }
        building.transform.parent = editUI.parentBuildings.transform;
        
        building.isPointerDown = true;
        building.editModesObject = editUI.editModes.gameObject;
        building.editModes = editUI.editModes;
        building.EditModeActive();
        data.isBuilded = true;
        
        if (editUI.BM.GetBuildingAmount(code) - editUI.BM.GetBuildedAmount(code) > 0)
            infoText.text = (editUI.BM.GetBuildingAmount(code) - editUI.BM.GetBuildedAmount(code)) + " 개";
        else Destroy(gameObject);
    }
}
