using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ForestCombineMode : MonoBehaviour
{
    [SerializeField] private DefaultPanel _combinePanel;
    [SerializeField] private DduduInfoPopup _dduduInfoPanel;

    private bool isCombineMode = false;
    private Ddudu selectedDdudu = null;
    private List<RaycastResult> results = new List<RaycastResult>();

    public void OnCombineButtonClick()
    {
        var dduduList = DduduManager.Instance.DduduObjects;
        if (!isCombineMode)
        {
            foreach (var ddudu in dduduList)
            {
                ddudu.SetOnPointerDownAction(OnPointerDownAction);
                ddudu.SetOnPointerUpAction(OnPointerUpAction);
            }
        }
        else
        {
            foreach (var ddudu in dduduList)
            {
                ddudu.RemoveOnPointerDownAction();
                ddudu.RemoveOnPointerUpAction();
            }
        }
        isCombineMode = !isCombineMode;
    }

    private void OnPointerDownAction(Ddudu curDdudu)
    {
        selectedDdudu = curDdudu;
    }

    private void OnPointerUpAction()
    {
        results.Clear();
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Ddudu curDdudu = null; 
        EventSystem.current.RaycastAll(eventData, results);
        Debug.Log(results.Count);
        foreach (var result in results)
        {
            result.gameObject.TryGetComponent<Ddudu>(out var ddudu);
            if (ddudu == selectedDdudu) continue;
            else 
            {
                curDdudu = ddudu;
                break;
            }
        }
        if (selectedDdudu == null || curDdudu == null || selectedDdudu == curDdudu) return;
        if (!DduduRecipeManager.Instance.TryGetResult(
            selectedDdudu.data.info.code,
            curDdudu.data.info.code,
            out var resultCode)) return;

        var resultInfo = DduduManager.Instance.GetInfo(resultCode);
        _combinePanel.Activate("뚜두 이름", "뚜두 설명", "두두 이미지", "두두 보석 이미지", "두두 관심 건물 이미지");
        // DduduSpawner.SpawnDdudu(resultId, Vector3.zero, true);
        // DduduManager.Instance.RemoveData(curDdudu.data.id);
        // DduduManager.Instance.RemoveData(selectedDdudu.data.id);
    }
}
