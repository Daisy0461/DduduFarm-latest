using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DduduMovement : MonoBehaviour
{
    [Header("DduduMovement")]
    public float speed;
    protected Vector2 dir;
    private Animator animator;

    protected virtual void Start()
    {
        dir = Vector2.zero;
        animator = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected virtual void Update() 
    {
        HandleAnimation();
    }

    public void HandleAnimation()
    {
        // if (animator == null) return;
        // if (dir.x != 0 || dir.y != 0)
            // AnimateMovement();
        // else
            // animator.SetLayerWeight(1, 0);
    }

    public void Move()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    public void AnimateMovement()
    {
        animator.SetLayerWeight(1, 1);
        
        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);
    }
}
