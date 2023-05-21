using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using ResearchType = ResearchManager.ResearchType;

[Serializable]
public class ResearchItemPair
{
    [SerializeField] private ResearchItem _researchItem;
    [SerializeField] private Image[] _postResearchLines;

    public void SetResearchItemPair(int researchId, bool canPostResearch, Color lockColor)
    {
        _researchItem.SetResearchItem(researchId);
        foreach (var item in _postResearchLines)
        {
            item.color = canPostResearch ? Color.white : lockColor;
        }
    }

    public void SetOnClickAction(Action<int> action)
    {
        _researchItem.SetOnResearchItemClick(action);
    }
}

public class ResearchCategoryUI : MonoBehaviour
{
    [SerializeField] private ResearchItemPair[] _researchItems;
    [SerializeField] private Color _lockColor;
    [SerializeField] private RectTransform _contentRect;

    private Action<int> _onItemClickAction;

    public void SetResearchCategoryUI(ResearchType type)
    {
        if (_contentRect != null)
        {
            _contentRect.anchoredPosition = Vector2.zero;
        }

        var researchInfos = ResearchManager.Instance.GetResearchInfosByResearchType(type);
        var minCount = Mathf.Min(_researchItems.Length, researchInfos.Count);
        for (int index = 0; index < minCount; index++)
        {
            var researchInfo = researchInfos[index];
            var researchData = ResearchManager.Instance.GetDataExactly(researchInfo.code);
            _researchItems[index].SetResearchItemPair(researchInfo.code, researchData.IsResearched, _lockColor);
            _researchItems[index].SetOnClickAction(_onItemClickAction);
        }
    }

    public void SetOnClickAction(Action<int> action)
    {
        _onItemClickAction = action;
    }
}
