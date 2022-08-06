using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;
    
	public GameObject editModesObject;
	public EditModes editModes;
    public Vector3 prePos;

    public bool isPointerDown = false;  // EditMode인가?
	private float minClickTime = 1f;
	private bool isClick;
	private float clickTime;

    #region Build Methods
    
    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        
        if (GridBuildingSystem.current.CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.current.TakeArea(areaTemp);
    }

    #endregion

    #region Touch Methods & Edit Mode Set up

    public void ButtonDown()
	{
		isClick = true;
		StartCoroutine(PointerLongDown());
	}

	public void ButtonUp()
	{
		isClick = false;
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
		editModes.selectedBuildingObject = this.gameObject;
		editModes.selectedBuilding = this;
		editModes.selectedBuildingRenderer = this.GetComponent<SpriteRenderer>();
		this.editModesObject.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
		this.editModesObject.SetActive(true);
	}

    #endregion
}
