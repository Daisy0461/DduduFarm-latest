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
    }
}
