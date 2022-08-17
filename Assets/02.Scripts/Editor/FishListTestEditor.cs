using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FishListTest))]
public class FishListTestEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        FishListTest test = (FishListTest)target;
        if (GUILayout.Button("Set Fish"))
        {
            test.SetFish();
        }

        if (GUILayout.Button("Set Crop"))
        {
            test.SetCrop();
        }
        
        test.val = EditorGUILayout.IntField("money", test.val);
        if (GUILayout.Button("Set Money"))
        {
            test.SetMoney();
        }

        if (GUILayout.Button("Set Buildings"))
        {
            test.SetBuildings();
        }
    }
}
