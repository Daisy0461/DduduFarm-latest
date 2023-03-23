using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DduduDataPopup : DefaultPanel
{
    public override void Activate(params object[] objs)
    {
        var dduduData = DduduManager.Instance.GetData((int)objs[0]);
        var dduduInfo = dduduData.info;
        var buildingImgPath = BuildingManager.Instance.GetInfo(dduduData.interest).imgPath;
        var gem1ImgPath = ItemManager.Instance.GetInfo(dduduInfo.gem1Id).imgPath;
        var gem2ImgPath = ItemManager.Instance.GetInfo(dduduInfo.gem2Id)?.imgPath;

        _text[0].text = dduduInfo.name;
        _text[1].text = dduduInfo.note;
        _images[0].sprite = Resources.Load<Sprite>(dduduInfo.imgPath);
        _images[1].sprite = Resources.Load<Sprite>(buildingImgPath);
        _images[2].sprite = Resources.Load<Sprite>(gem1ImgPath);
        if (gem2ImgPath != string.Empty)
        {
            _images[3].sprite = Resources.Load<Sprite>(gem2ImgPath);
        }
        else
        {
            _images[3].gameObject.SetActive(false);
        }
        base.Activate();
    }
}
