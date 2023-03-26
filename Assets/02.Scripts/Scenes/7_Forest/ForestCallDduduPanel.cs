using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestCallDduduPanel : DefaultPanel
{
    [SerializeField] private Vector3 _spawnPos;
    [SerializeField] private Transform _spawnParent;
    [SerializeField] private ForestCallDduduItem[] _items;

    private void Awake()
    {
        for (int index = 0; index < _items.Length; index++) 
        { 
            _items[index].SetData((int)DataTable.Ddudu + index + 1, _spawnPos, _spawnParent);
            _items[index].SetOnClickAction(base.OnCloseButtonClick);
        }
    }

}
