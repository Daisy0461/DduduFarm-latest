using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(DduduPrefabMaker))]
public class DduduPrefabMakerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        var t = (DduduPrefabMaker)target;
        if (GUILayout.Button("Duplicate Prefab"))
        {
           t.DuplicatePrefab();
        }
    }
}


[ExecuteInEditMode]
public class DduduPrefabMaker : MonoBehaviour
{
    [SerializeField] private static int startNum = 121;
    public GameObject targetObject;    
    
    public void DuplicatePrefab()
    {
        var obj = PrefabUtility.InstantiatePrefab(targetObject) as GameObject;
        for (int i = startNum; i <= 165; i++)
        {
            var savePath = $"Assets/Resources/DduduPrefab/Ddudu_{i}.prefab";
            PrefabUtility.SaveAsPrefabAsset(obj, savePath);
        }
        
    }
}

#endif