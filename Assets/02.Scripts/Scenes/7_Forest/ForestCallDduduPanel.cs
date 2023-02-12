using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestCallDduduPanel : DefaultPanel
{
    [SerializeField] private Vector2 _spawnPos;
    [SerializeField] private Transform _spawnParent;

	public override void OnButtonClick(int index)
	{
		base.OnButtonClick(index);
        int id = DduduManager.Instance.AddData((int)DataTable.Ddudu + index + 1);
        var newDdudu = DduduSpawner.SpawnDdudu(id, _spawnPos);
        newDdudu.transform.SetParent(_spawnParent);
	}
}
