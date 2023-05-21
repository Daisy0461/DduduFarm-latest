using UnityEngine;
using UnityEngine.UI;
using ResearchType = ResearchManager.ResearchType;

public class ResearchPanel : MonoBehaviour
{
    [SerializeField] private ResearchCategoryUI[] _researchCategoryUIs;
    [SerializeField] private Image[] _categoryButton;
    [SerializeField] private ResearchCompleteUI _researchCompleteUI;
    [SerializeField] private ResearchProgressUI _researchProgressUI;

    private ResearchType _curType;

#region Research Setting

    private void Start() 
    {
        ShowCategoryUI(ResearchType.Farm);
    }

    private void ShowCategoryUI(ResearchType type)
    {
        _curType = type;

        foreach (var researchCategoryUI in _researchCategoryUIs)
        {
            researchCategoryUI.gameObject.SetActive(false);
        }

        if ((int)type >= _researchCategoryUIs.Length)
        {
            Debug.LogError($"type {type} is bigger than researchCatgoryUIs Length {_researchCategoryUIs.Length}");
            return;
        }
        _researchCategoryUIs[(int)type].gameObject.SetActive(true);
        _researchCategoryUIs[(int)type].SetOnClickAction(OnResearchItemClick);
        _researchCategoryUIs[(int)type].SetResearchCategoryUI(type);
        for (int index = 0; index < _categoryButton.Length; index++)
        {
            _categoryButton[index].color = index == (int)type ? Color.white : Color.grey;
        }
    }

    private void OnResearchItemClick(int researchId)
    {
        var researchData = ResearchManager.Instance.GetData(researchId);
        if (researchData == null) Debug.LogError("reserarchData Is Null");
        if (researchData.IsResearched) // 연구 완료
        {
            _researchCompleteUI.Activate(researchId);
            _researchProgressUI.SetOnCloseAction(() => ShowCategoryUI(_curType));
        }
        else 
        {
            var researchInfo = ResearchManager.Instance.GetInfo(researchId);
            var preResearchInfo = ResearchManager.Instance.GetInfo(researchInfo.preCode);
            var preResearchData = ResearchManager.Instance.GetData(researchInfo.preCode);
            if (preResearchInfo == null || (preResearchData != null && preResearchData.IsResearched)) // 연구 가능
            {
                _researchProgressUI.Activate(researchId);
                _researchProgressUI.SetOnCloseAction(() => ShowCategoryUI(_curType));
            }
            else // 연구 불가
            {
                // researchId 에 해당하는 연구 정보
                var errorPopup = PopupManager.Instance.GetErrorPopup();
                if (errorPopup == null)
                {
                    Debug.LogError("Error Popup Is None");
                    return;
                }
                errorPopup.SetText("{0}.을 연구해야 합니다.".FormatK(preResearchInfo.name));
                errorPopup.Activate();
            }
        }
    }

#endregion // Research Setting

#region Button

    public void OnFarmButtonClick()
    {
        ShowCategoryUI(ResearchType.Farm);
    }

    public void OnFishButtonClick()
    {
        ShowCategoryUI(ResearchType.Fish);
    }

    public void OnStructureButtonClick()
    {
        ShowCategoryUI(ResearchType.Structure);
    }

    public void OnInventoryButtonClick()
    {
        ShowCategoryUI(ResearchType.Inventory);
    }

#endregion // Button
}
