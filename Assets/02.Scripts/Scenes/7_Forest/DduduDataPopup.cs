using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DduduDataPopup : DefaultPanel
{
    [Header("Ddudu Data Popup")][Space(8)]
    [SerializeField] private TwoButtonPopup _twoButtonPopup;

    private int _dduduId;

    public void Activate() // TODO: ��ȫ�� Activate �� �� ����
    {
        Activate(null);
    }

    public override void Activate(params object[] objs)
    {
        _dduduId = objs == null ? _dduduId : (int)objs[0];

        var dduduData = DduduManager.Instance.GetData(_dduduId);
        var dduduInfo = dduduData.info;
        var buildingImgPath = BuildingManager.Instance.GetInfo(dduduData.interest)?.imgPath; // TODO : �������� ������ �ѵ��� �ǹ� �̹����� �ҷ����� ���Ѵ�. ��?
        var gem1ImgPath = ItemManager.Instance.GetInfo(dduduInfo.gem1Id).imgPath;
        var gem2ImgPath = ItemManager.Instance.GetInfo(dduduInfo.gem2Id)?.imgPath;

        _text[0].text = dduduInfo.name;
        _text[1].text = dduduInfo.note;
        _images[0].sprite = Resources.Load<Sprite>(dduduInfo.imgPath);
        _images[1].sprite = Resources.Load<Sprite>(buildingImgPath);
        _images[2].sprite = Resources.Load<Sprite>(gem1ImgPath);
        if (gem2ImgPath != null && gem2ImgPath != string.Empty)
        {
            _images[3].sprite = Resources.Load<Sprite>(gem2ImgPath);
        }
        else
        {
            _images[3].gameObject.SetActive(false);
        }
        base.Activate();
    }

    public void SetDduduIdExtern(int dduduId)
    {
        _dduduId = dduduId;
    }

    public void OnRemoveButtonClick()
    {
        _twoButtonPopup.SetAction(LeavePositiveAction, LeaveNegativeAction);
        _twoButtonPopup.SetOnCloseAction(LeavePositiveCloseAction);
        _twoButtonPopup.Activate("���� ���������ðڽ��ϱ�?");
        
        OnCloseButtonClick();
    }

    public override void OnCloseButtonClick()
    {
        base.OnCloseButtonClick();
    }

    private void LeavePositiveAction()
    {
        DduduManager.Instance.RemoveData(_dduduId);
    }

    private void LeaveNegativeAction()
    {
        return;
    }

    private void LeavePositiveCloseAction()
    {
        if (DduduManager.Instance.IsDataExist(_dduduId)) return;
        Debug.Log("���������� ����Ʈ ���");
    }
}
