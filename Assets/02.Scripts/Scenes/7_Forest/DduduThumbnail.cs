using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DduduThumbnail : MonoBehaviour
{
    [SerializeField] private Image _thumbnailImage;
    [SerializeField] private Text _thumbnailText;
    [SerializeField] private DduduDataPopup _dataPopup;
    [SerializeField] private DduduInfoPopup _infoPopup;

    private DduduData _data;
    private DduduInfo _info;

    public void SetData(int dataId)
    {
        _data = DduduManager.Instance.GetData(dataId);
        _info = _data?.info ?? DduduManager.Instance.GetInfo(dataId);

        _thumbnailImage.sprite = Resources.Load<Sprite>(_info.imgPath);
        _thumbnailText.text = _info.name;
    }

    public void OnThumbnailClick()
    {
        if (_dataPopup != null)
        {
            _dataPopup.Activate(_data.id);
        }
        if (_infoPopup != null)
        {
            _infoPopup.Activate(_info.code);
        }
    }
}
