using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DduduInfoPopup : DefaultPanel
{
    public override void Activate(params object[] objs)
    {
        _text[0].text = (string)objs[0];    // 뚜두 이름
        _text[1].text = (string)objs[1];   // 뚜두 설명
        _images[0].sprite = Resources.Load<Sprite>((string)objs[2]);    // 뚜두 이미지
        _images[1].sprite = Resources.Load<Sprite>((string)objs[3]);    // 뚜두 보석 이미지
        _images[2].sprite = Resources.Load<Sprite>((string)objs[4]);    // 뚜두 관심 건물 이미지
        base.Activate();
    }
}
