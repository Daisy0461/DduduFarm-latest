
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ShopUpdateTest))]
public class ShopUpdateTestEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    
        ShopUpdateTest t = (ShopUpdateTest)target;
        
        GUILayout.Label("update time");
        t.hour = EditorGUILayout.IntField("hour", t.hour);
        t.minute = EditorGUILayout.IntField("minute", t.minute);
        t.second = EditorGUILayout.IntField("second", t.second);
        t.millisecond = EditorGUILayout.IntField("millisecond", t.millisecond);
        if (GUILayout.Button("Time Update"))
        {
            t.UpdateTime();
        }

        GUILayout.Label("tempTime : \t" + t.tempTime);
    }
}
