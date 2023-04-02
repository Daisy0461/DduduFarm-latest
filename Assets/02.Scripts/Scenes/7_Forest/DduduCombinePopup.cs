using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DduduCombinePopup : DefaultPanel
{
    [SerializeField] private DduduThumbnail[] _thumbnails;

    private int _resultCode;
    private int _selectedDduduId;
    private int _curDduduId;
    
    public override void Activate(params object[] objs)
    {
        var selectedDdudu = DduduManager.Instance.GetData((int)objs[0]);
        var curDdudu = DduduManager.Instance.GetData((int)objs[1]);
        var resultInfo = DduduManager.Instance.GetInfo((int)objs[2]);

        _text[0].text = selectedDdudu.info.name;
        _images[0].sprite = Resources.Load<Sprite>(selectedDdudu.info.imgPath);
        _text[1].text = curDdudu.info.name;
        _images[1].sprite = Resources.Load<Sprite>(curDdudu.info.imgPath);
        _text[2].text = resultInfo.name;
        _images[2].sprite = Resources.Load<Sprite>(resultInfo.imgPath);

        if (_thumbnails.Length > 0) _thumbnails[0]?.SetData(selectedDdudu.id);
        if (_thumbnails.Length > 1) _thumbnails[1]?.SetData(curDdudu.id);
        if (_thumbnails.Length > 2) _thumbnails[2]?.SetData(selectedDdudu.id);

        _resultCode = resultInfo.code;
        _selectedDduduId = selectedDdudu.id;
        _curDduduId = curDdudu.id;

        base.Activate(objs);
    }

    public void OnCombineButtonClick()
    {
        DduduManager.SpawnDdudu(_resultCode, Vector3.zero, true);
        DduduManager.Instance.RemoveData(_curDduduId);
        DduduManager.Instance.RemoveData(_selectedDduduId);
        base.OnCloseButtonClick();
    }
}
