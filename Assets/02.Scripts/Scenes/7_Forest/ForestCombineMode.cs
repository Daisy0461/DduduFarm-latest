using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ForestCombineMode : MonoBehaviour
{
    [SerializeField] private DduduCombinePopup _combinePopup;
    [SerializeField] private GameObject _dataButtonObject;
    [SerializeField] private DduduDataPopup _dataPopup;

    private bool isCombineMode = false;
    private Ddudu selectedDdudu = null;
    private List<RaycastResult> results = new List<RaycastResult>();

    public void OnEnable()
    {
        DduduManager.SetSpawnPointerDownAction(OnInfoButtonActiveAction);
    }

    public void OnCombineButtonClick()
    {
        var dduduList = DduduManager.Instance.DduduObjects;
        if (!isCombineMode)
        {
            foreach (var ddudu in dduduList)
            {
                ddudu.RemoveOnPointerDownAction();

                ddudu.CombineMpdeDimmed(true);
                ddudu.SetOnPointerDownAction(OnPointerDownAction);
                ddudu.SetOnPointerUpAction(OnPointerUpAction);
            }
        }
        else
        {
            foreach (var ddudu in dduduList)
            {
                ddudu.CombineMpdeDimmed(false);
                ddudu.RemoveOnPointerDownAction();
                ddudu.RemoveOnPointerUpAction();

                ddudu.SetOnPointerDownAction(OnInfoButtonActiveAction);
            }
        }
        isCombineMode = !isCombineMode;
    }

    public void OnInfoButtonActiveAction(Ddudu ddudu)
    {
        _dataButtonObject.SetActive(true);
        _dataPopup.SetOnCloseAction(() => _dataButtonObject.SetActive(false));
        _dataPopup.SetDduduIdExtern(ddudu.data.id, ddudu.transform);
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

        _combinePopup.Activate(selectedDdudu.data.id, curDdudu.data.id, resultCode);
        OnCombineButtonClick();
    }
}
