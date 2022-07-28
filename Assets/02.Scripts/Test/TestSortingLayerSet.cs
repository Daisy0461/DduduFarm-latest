using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSortingLayerSet : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sortingOrder = 
                    (int)(this.transform.position.y * -10) + (int)(transform.position.x * -5);
    }
}
