using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditBuildingGrid : MonoBehaviour
{
    EditBuilding parentEditBuilding;
    [SerializeField] EditModes editModes;
    public SpriteRenderer spriteRenderer;

    private void Start() 
    {
        parentEditBuilding = GetComponentInParent<EditBuilding>();    
        editModes = parentEditBuilding.editModes;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	private void OnTriggerStay2D(Collider2D other)
	{
		if (parentEditBuilding.isPointerDown == false) return;
		if (other.gameObject.layer == 8) return;

		Color color = Color.white;
		color.a = 0.5f;
		editModes.btnBlock.GetComponent<Image>().color = color;
        spriteRenderer.color = Color.red;
		editModes.isAbleToFix = false;
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (parentEditBuilding.isPointerDown == false) return;
		if (other.gameObject.layer == 8) return;
		
		Color color = Color.white;
		color.a = 1f;
		editModes.btnBlock.GetComponent<Image>().color = color;
        spriteRenderer.color = Color.green;
		editModes.isAbleToFix = true;
	}

}
