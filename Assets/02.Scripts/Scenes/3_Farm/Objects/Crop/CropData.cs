using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop", menuName = "Farm Game/Crop", order = 2)]

public class CropData : ScriptableObject
{
    public new string name = "Crop";
    public GameObject CropPrefab;

    public float maxGrowth = 100f;
}
