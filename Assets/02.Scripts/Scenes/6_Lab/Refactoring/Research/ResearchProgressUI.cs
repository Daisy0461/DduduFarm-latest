using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchProgressUI : DefaultPanel
{
    [SerializeField] private UIItem[] _requireItems;
    [SerializeField] private GameObject _researchButton;
    [SerializeField] private GameObject _researchLockButton;

    private int _researchId;

    public override void Activate(params object[] objs)
    {
        _researchId = (int)objs[0];

        var researchInfo = ResearchManager.Instance.GetInfo(_researchId);
        if (researchInfo == null)
        {
            Debug.LogError("researchInfo is null");
            return;
        }

        _titleText[0].text = researchInfo.name;
        _images[0].sprite = Resources.Load<Sprite>(researchInfo.imgPath);
        _text[0].text = researchInfo.note;
        _text[1].text = $"{researchInfo.level}";
        _text[2].text = $"{researchInfo.researchValue}";

        var isUnLock = true;
        for (int index = 0; index <  _requireItems.Length; index++)
        {
            if (researchInfo.requireMaterial.Count <= index)
            {
                _requireItems[index].SetEmpty();
                continue;
            }
            var itemInfo = ItemManager.Instance.GetInfo(researchInfo.requireMaterial[index].code);
            var itemData = ItemManager.Instance.GetData(researchInfo.requireMaterial[index].code);
            var requireCount = researchInfo.requireMaterial[index].count;
            var itemCount = itemData == null ? 0 : itemData.amount;
            var countString = itemCount < requireCount ? $"<color=#FF231C>{itemCount}</color>" : $"{itemCount}";
            _requireItems[index].SetSprite(itemInfo.imgPath);
            _requireItems[index].SetText($"{countString} / {requireCount}");
            _requireItems[index].gameObject.SetActive(true);

            isUnLock = isUnLock && requireCount <= itemCount;
        }
        _researchButton.SetActive(isUnLock);
        _researchLockButton.SetActive(!isUnLock);

        base.Activate(objs);
    }

    public void OnResearcchButtonClick()
    {
        var researchInfo = ResearchManager.Instance.GetInfo(_researchId);
        foreach ((int id, int count) in researchInfo.requireMaterial)
        {
            ItemManager.Instance.RemoveData(id, count);
        }

        ResearchManager.Instance.ResearchTypeStatus(_researchId);
        OnCloseButtonClick();
    }
}
