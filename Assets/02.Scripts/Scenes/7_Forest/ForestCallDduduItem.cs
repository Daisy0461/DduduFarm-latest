using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForestCallDduduItem : UIItem
{
    [SerializeField] private Image _dduduImage;
    [SerializeField] private Image[] _materialImages;
    [SerializeField] private Text[] _materialTexts;
    [SerializeField] private GameObject _effectObject;

    private Vector3 _spawnPos;
    private Transform _spawnParent;

    public override void SetData(params object[] objs)
    {
        base.SetData(objs);
        _code = (int)objs[0];
        _spawnPos = (Vector3)objs[1];
        _spawnParent = (Transform)objs[2];

        var dduduImgPath = DduduManager.Instance.GetInfo(_code).imgPath;
        _dduduImage.sprite = Resources.Load<Sprite>(dduduImgPath);
        
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        var ddudu = DduduSpawner.SpawnDdudu(_code, _spawnPos, true);
        ddudu.transform.SetParent(_spawnParent);
        if (_effectObject != null)
        {
            _effectObject.SetActive(true);
        }
    }
}
