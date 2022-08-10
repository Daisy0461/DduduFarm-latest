using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEdit : MonoBehaviour
{
	public GridBuildingSystem gridBuildingSystem;
	public GameObject editModesObject;
	public EditModes editModes;
	
	GameObject obj;
	public Building building;

	// 생성
    public void OnclickBuildingInstantiate(GameObject buildingPrefab)
	{
		building = gridBuildingSystem.InitializeWithBuilding(buildingPrefab);

		building.editModesObject = editModesObject;
		building.editModes = editModes;
		building.isPointerDown = true;
		building.EditModeActive();
	}

	private void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			gridBuildingSystem.gameObject.SetActive(true);
			gridBuildingSystem.temp = building;
			gridBuildingSystem.LongClickBuilding();
		}
	}
}
