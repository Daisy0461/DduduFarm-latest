using System.Collections;
using UnityEngine;

public class EditBuilding : MonoBehaviour
{
	ItemManager IM;
	public BuildingData data;

	public bool isPointerDown = false;  // EditMode인가?
	private float minClickTime = 1f;
	private bool isClick;
	private float clickTime;

	public Vector3 prePos;
	public GameObject editModesObject;
	public EditModes editModes;
	public EditBuildingGrid grid;
	public PopupBuilding popupBuilding;

	[SerializeField] private Common common;
	[SerializeField] public Craft craft;

	private void Awake()
	{
		if (common == null) common = GetComponent<Common>();
		if (craft == null) craft = GetComponent<Craft>();
		grid = GetComponentInChildren<EditBuildingGrid>();
	}

	private void Start() 
	{
		IM = ItemManager.Instance;
		this.GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y * -10);
		editModes = editModesObject.GetComponent<EditModes>();
	}

	public void ButtonDown()
	{
		isClick = true;
		StartCoroutine(PointerLongDown());
	}

	public void ButtonUp()
	{
		isClick = false;
		if (!isPointerDown)  // 이동이 아니라 골드 획득 혹은 팝업 노출
		{
			if (common != null && common.goldBtn.activeSelf) // 일반 건물, 작업 완료 시 골드 획득
				common.OnClickGoldBtn();
			else
			{
				popupBuilding.common = common;
				popupBuilding.craft = craft;
				popupBuilding.gameObject.SetActive(true);
				popupBuilding.RenewPanel(popupBuilding.index);	
			}
		}
	}

	IEnumerator PointerLongDown()
	{
		while (isClick)
		{
			clickTime += Time.deltaTime;
			if (clickTime >= minClickTime && !isPointerDown)
				IsPointerDown();
			yield return null;
		}
		clickTime = 0;
	}

	public void IsPointerDown()
	{
		isPointerDown = true;
		prePos = transform.position;
		EditModeActive();
	}

	public void EditModeActive()
	{	
		editModes.selectedBuilding = this.gameObject;
		editModes.selectedEditBuilding = this;
		if (grid == null) grid = GetComponentInChildren<EditBuildingGrid>();
		editModes.selectedBuildingGrid = grid;
		editModes.selectedBuildingRenderer = this.GetComponent<SpriteRenderer>();
		this.editModesObject.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
		this.editModesObject.SetActive(true);
	}
}
