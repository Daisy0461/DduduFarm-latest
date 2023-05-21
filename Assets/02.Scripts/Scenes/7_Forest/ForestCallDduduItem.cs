using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForestCallDduduItem : UIItem
{
    [SerializeField] private Image _dduduImage;
    [SerializeField] private Image[] _materialImages;
    [SerializeField] private Text[] _materialTexts;
    [SerializeField] private GameObject[] _materialObjects;
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

        var dduduInfo = ResearchManager.Instance.GetInfo(_code);
        var index = 0;
        foreach (var material in dduduInfo.requireMaterial)
        {
            var matId = material.code;
            var matAmount = material.count;
            _materialImages[index].sprite = Resources.Load<Sprite>(ItemManager.Instance.GetInfo(matId).imgPath);
            _materialTexts[index].text = matAmount.ToString();
            _materialObjects[index].SetActive(true);
            index++;
        }
        while (index <= dduduInfo.requireMaterial.Count)
        {
            _materialObjects[index].SetActive(false);
            index++;
        }
    }

    public override void OnButtonClick()
    {
        var ddudu = DduduManager.SpawnDdudu(_code, _spawnPos, true);
        ddudu.transform.SetParent(_spawnParent);
        base.OnButtonClick();
    }
}
