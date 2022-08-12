using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBuildings : MonoBehaviour
{
    [SerializeField] DduduSpawner DS;
    [SerializeField] EditModes editModes;
    
    [SerializeField] GameObject[] EditCommonPrefab;
    [SerializeField] GameObject[] EditCraftPrefab;
    [SerializeField] PopupBuilding[] popupBuildings;

    private Quaternion rotate = new Quaternion();
    
    void Start()
    {
       LoadBuildingsOnScene();
    }

    public void LoadBuildingsOnScene()
	{
        foreach (var data in BuildingManager.Instance.GetDataList())
        {
            if (data.isBuilded == false) continue;

            int code = data.info.code;
            Building building;
            GameObject buildingObject;
            Vector3 pos = new Vector3(data.x, data.y, data.z);
            
            if (code <= 50)
            {
                buildingObject = Instantiate(EditCommonPrefab[code%(int)DataTable.Craft-1]);
                Common common = buildingObject.GetComponent<Common>();
                common.data = data;
                common.popupBuilding = popupBuildings[0];
            }
            else
            {
                buildingObject = Instantiate(EditCraftPrefab[code%(int)DataTable.Craft-1]);
                Craft craft = buildingObject.GetComponent<Craft>();
                craft.DS = DS;
                craft.data = data;
                craft.popupBuilding = popupBuildings[1];
            }
            building = buildingObject.GetComponent<Building>();
            building.editModesObject = editModes.gameObject;
            building.editModes = editModes;
            building.transform.SetPositionAndRotation(pos, rotate);
            building.transform.parent = this.transform;
        }
	}
}
