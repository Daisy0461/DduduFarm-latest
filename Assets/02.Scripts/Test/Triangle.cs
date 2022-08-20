using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    MeshRenderer rend;
    public int sortinglayerId;
    public int sortingorder;
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        // gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = new Vector3[] {new Vector3(0,0,0), new Vector3(30,52,0), new Vector3(60, 0, 0)};
        mesh.uv = new Vector2[]{new Vector2(0, 0), new Vector2(0, 1), new Vector2(0.5f, 1)};
        mesh.triangles = new int[] {0, 1, 2};    

        rend = GetComponent<MeshRenderer>();
        rend.sortingOrder = 1000;  
        rend.sortingLayerName = "UI";      
    }

    private void Update() {
        if (rend.sortingLayerID != sortinglayerId)
        {
            rend.sortingLayerID = sortinglayerId;
        }
        if (rend.sortingOrder != sortingorder)
        {
            rend.sortingOrder = sortingorder;
        }
    }
}
