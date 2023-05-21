using System;
using UnityEngine;
using UnityEngine.UI;

public class ResearchItem : MonoBehaviour
{
    [SerializeField] private Color _researchLockColor;
    [SerializeField] private Image _researchImage;
    [SerializeField] private Text _researchText;

    private int _researchId;
    private Action<int> _researchItemClick = null;

    public void SetOnResearchItemClick(Action<int> action)
    {
        _researchItemClick = action;
    }

    public void SetResearchItem(int researchId)
    {
        _researchId = researchId;

        var researchInfo = ResearchManager.Instance.GetInfo(researchId);
        if (researchInfo == null) 
        {
            Debug.LogError($"researchInfo is null {researchId}");
            return;
        }

        _researchImage.sprite = Resources.Load<Sprite>(researchInfo.imgPath);
        _researchText.text = researchInfo.name;

        var userData = ResearchManager.Instance.GetDataExactly(researchId);
        if (userData == null)
        {
            Debug.LogError($"user research data is null {researchId}");
            return;
        }

        _researchImage.color = userData.IsResearched ? Color.white : _researchLockColor;
    }

    public void OnResearchItemClick()
    {
        _researchItemClick.Invoke(_researchId);
    }
}
