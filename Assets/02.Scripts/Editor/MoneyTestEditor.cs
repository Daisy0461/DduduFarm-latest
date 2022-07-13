using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoneyTest))]
public class MoneyTestEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        MoneyTest test = (MoneyTest)target;
        if (GUILayout.Button("Update Money"))
        {
            test.UpdateMoney();
        }
    }
}
