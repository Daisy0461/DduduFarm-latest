using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DduduInfoPopup : DefaultPanel
{
    public override void Activate(params object[] objs)
    {
        _text[0].text = (string)objs[0];    // �ѵ� �̸�
        _text[1].text = (string)objs[1];   // �ѵ� ����
        _images[0].sprite = Resources.Load<Sprite>((string)objs[2]);    // �ѵ� �̹���
        _images[1].sprite = Resources.Load<Sprite>((string)objs[3]);    // �ѵ� ���� �̹���
        _images[2].sprite = Resources.Load<Sprite>((string)objs[4]);    // �ѵ� ���� �ǹ� �̹���
        base.Activate();
    }
}
