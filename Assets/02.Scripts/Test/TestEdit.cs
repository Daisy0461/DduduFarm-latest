using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEdit : MonoBehaviour
{
	public GridBuildingSystem gridBuildingSystem;
	public GameObject editModesObject;
	public EditModes editModes;
	
	GameObject obj;
	Building building;

	// 생성
    public void OnclickBuildingInstantiate(GameObject buildingPrefab)
	{
		obj = gridBuildingSystem.InitializeWithBuilding(buildingPrefab);
		building = obj.GetComponent<Building>();

		building.editModesObject = editModesObject;
		building.editModes = editModes;
		building.isPointerDown = true;
		building.EditModeActive();
	}

	// 편집 모드 / 확인 EditModes - OnClickFix


	// 다른 건물 생성
	
	// 겹치기 - 빨강 구역, 초록 구역, 노랑 구역
	
	// 저장
	
	// 로드

}
