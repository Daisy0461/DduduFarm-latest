using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public float speed = 0.3f;
    Vector2 dir;

    private void FixedUpdate() 
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void Update()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");

        this.GetComponent<SpriteRenderer>().sortingOrder = 
                                    (int)(this.transform.position.y * -10) + (int)(transform.position.x * -5);
    
        
    }
}
