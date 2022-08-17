using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditUI : MonoBehaviour
{
    [HideInInspector] public BuildingManager BM;
    public DduduSpawner DS;
    public GridBuildingSystem tilemap;
    
    [Header("Edit")]
	public GameObject parentBuildings;                  // 건물을 하이어라키에서 모아놓기 위한 부모 오브젝트
	public EditModes editModes;                        // 편집 모드를 위한 원형 모드s
    
    [Header("Building Property")]
	public PopupBuilding[] popupBuildings;              // 건물 정보 팝업 0-일반, 1-공방

    [Header("Instantiate")]
    public GameObject EditItemPanelPrefab;              // 편집 상품 프리팹
    public GameObject EditContent;                      // 편집 스크롤뷰 content

    public AudioSource audioSource;

    private void Start() 
    {
        BM = BuildingManager.Instance;
        foreach (var data in BM.GetUniqueUnBuildedBuilding())
            CreateEditBtnUI(data);
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

        newObj.GetComponent<Button>().onClick.AddListener( () => Quit() );

        if ((BM.GetBuildingAmount(code) - BM.GetBuildedAmount(code)) > 0)   // 배치 안 하고 남은 게 있을 때
            newObj.SetActive(true);
        else newObj.SetActive(false);
    }

    public void Quit()
    {
        audioSource.Play();
        gameObject.SetActive(false);
    }
}
