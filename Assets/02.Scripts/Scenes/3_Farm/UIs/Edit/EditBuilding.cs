// using System.Collections;
// using UnityEngine;

// public class EditBuilding : MonoBehaviour
// {
// 	public BuildingData data;

// 	public bool isPointerDown = false;  // EditMode인가?
// 	private float minClickTime = 1f;
// 	private bool isClick;
// 	private float clickTime;

// 	public Vector3 prePos;
// 	public GameObject editModesObject;
// 	public EditModes editModes;

// 	private void Start() 
// 	{
// 		editModes = editModesObject.GetComponent<EditModes>();
// 	}

// 	public void ButtonDown()
// 	{
// 		isClick = true;
// 		StartCoroutine(PointerLongDown());
// 	}

// 	public void ButtonUp()
// 	{
// 		isClick = false;
// 	}

// 	IEnumerator PointerLongDown()
// 	{
// 		while (isClick)
// 		{
// 			clickTime += Time.deltaTime;
// 			if (clickTime >= minClickTime && !isPointerDown)
// 				IsPointerDown();
// 			yield return null;
// 		}
// 		clickTime = 0;
// 	}

// 	public void IsPointerDown()
// 	{
// 		isPointerDown = true;
// 		prePos = transform.position;
// 		EditModeActive();
// 	}

// 	public void EditModeActive()
// 	{	
// 		editModes.selectedBuildingObject = this.gameObject;
// 		editModes.selectedBuilding = this;
// 		editModes.selectedBuildingRenderer = this.GetComponent<SpriteRenderer>();
// 		this.editModesObject.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
// 		this.editModesObject.SetActive(true);
// 	}
// }
