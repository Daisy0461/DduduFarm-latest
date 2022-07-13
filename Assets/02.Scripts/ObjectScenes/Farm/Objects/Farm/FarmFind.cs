using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmFind : MonoBehaviour
{
    public FarmState[] farmStateList;

    void Start()
    {
        farmStateList = gameObject.GetComponentsInChildren<FarmState>();
    }
}