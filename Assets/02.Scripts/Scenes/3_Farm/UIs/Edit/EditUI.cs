using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditUI : MonoBehaviour
{
    [HideInInspector] public BuildingManager BM;
    public DduduSpawner DS;

    [Header("Edit")]
	public GameObject[] EditCommonPrefab;              // 편집에서 씬에 생성되는 게임오브젝트
	public GameObject[] EditCraftPrefab;
	public GameObject parentBuildings;                  // 건물을 하이어라키에서 모아놓기 위한 부모 오브젝트
	public GameObject editModes;                        // 편집 모드를 위한 원형 모드s
    
    [Header("Building Property")]
	public PopupBuilding[] popupBuildings;              // 건물 정보 팝업 0-일반, 1-공방

    [Header("Instantiate")]
    public GameObject EditItemPanelPrefab;              // 편집 상품 프리팹
    public GameObject EditContent;                      // 편집 스크롤뷰 content

    public AudioSource audioSource;
    private Quaternion rotate = new Quaternion();

    private void Start() 
    {
        BM = BuildingManager.Instance;
        LoadBuildingsOnScene();
        foreach (var data in BM.GetUniqueUnBuildedBuilding())
            CreateEditBtnUI(data);
    }

    public void LoadBuildingsOnScene()
	{
        foreach (var data in BM.GetDataList())
        {
            if (data.isBuilded == false) continue;

            int code = data.info.code;
            Building building;
            GameObject buildingObject = Instantiate(EditCommonPrefab[code%(int)DataTable.Craft-1]);
            Vector3 pos = new Vector3(data.x, data.y, data.z);
            
            if (code <= 50)
            {
                Common common = buildingObject.GetComponent<Common>();
                common.data = data;
                common.popupBuilding = popupBuildings[0];
            }
            else
            {
                Craft craft = buildingObject.GetComponent<Craft>();
                craft.DS = DS;
                craft.data = data;
                craft.popupBuilding = popupBuildings[1];
            }
            building = buildingObject.GetComponent<Building>();
            building.editModesObject = editModes;
            building.transform.SetPositionAndRotation(pos, rotate);
            building.transform.parent = parentBuildings.transform;
        }
	}

    public void CreateEditBtnUI(BuildingData data)
    {
        if (data.isBuilded == true) return;

        int code = data.info.code;
        var newObj = Instantiate(EditItemPanelPrefab, EditContent.transform);
        var editItem = newObj.GetComponent<EditItem>();
        editItem.data = data;
        editItem.editUI = this;
        editItem.infoText.text = (BM.GetBuildingAmount(code) - BM.GetBuildedAmount(code)) + " 개";
            
        Image newImg = newObj.transform.GetChild(1).GetComponent<Image>();
        newImg.sprite = Resources.Load<Sprite>((string)data.info.imgPath);

        Text newText = newObj.GetComponentInChildren<Text>();
        newText.text = data.info.name;

        newObj.GetComponent<Button>().onClick.AddListener( () => SoundPlay() );

        if ((BM.GetBuildingAmount(code) - BM.GetBuildedAmount(code)) > 0)   // 배치 안 하고 남은 게 있을 때
            newObj.SetActive(true);
        else newObj.SetActive(false);
    }

    public void SoundPlay()
    {
        audioSource.Play();
    }
}
