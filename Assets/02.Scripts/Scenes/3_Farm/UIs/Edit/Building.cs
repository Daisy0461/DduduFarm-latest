using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Building : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BoundsInt area;
    public bool Placed { get; private set; }
	public event Action gridActivate = null;
	
	//[HideInInspector]
	 public GameObject editModesObject;
	//[HideInInspector]
	 public EditModes editModes;
    [HideInInspector] public Vector3 prePos;
    [HideInInspector] public bool isPointerDown = false;  // EditMode인가?
	public SpriteRenderer render;
	private float minClickTime = 1f;
	private bool isClick;
	private float clickTime;

	#region Unity Methods
	
	private void Start() 
	{
		render.sortingOrder = (int)(this.transform.position.y * -10 + this.transform.position.x * -10);
		editModes.tilemap.gridActivate += CallBackActiveGrid;
	}

	#endregion

    #region Build Methods
    
	public void CallBackActiveGrid()
	{
		if (editModes.tilemap.temp != this)
			editModes.tilemap.CallbackSetBuildingTiles(this);
	}

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.WorldToCell(transform.position);
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
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.WorldToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.current.TakeArea(areaTemp);
    }

    #endregion

    #region Touch Methods & Edit Mode Set up

    public void OnPointerDown(PointerEventData e)
	{
		isClick = true;
		if (!editModes.tilemap.isActiveAndEnabled)
			StartCoroutine(PointerLongDown());
	}

	public void OnPointerUp(PointerEventData e)
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

		Placed = false;
		editModes.tilemap.temp = this;
		editModes.tilemap.LongClickBuilding();
		EditModeActive(); // longclick then callback
	}

    public void EditModeActive()
	{	
		editModes.selectedBuilding = this;
		editModesObject.transform.position = Camera.main.WorldToScreenPoint(render.transform.position);
		editModesObject.SetActive(true);
	}

    #endregion
}
