using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DduduMovement : MonoBehaviour
{
    [Header("DduduMovement")]
    public float speed;
    protected Vector2 dir;
    private Animator anim;

    protected virtual void Start()
    {
        dir = Vector2.zero;
        anim = GetComponent<Animator>();
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
        if (anim == null) return;
        if (dir.x != 0 || dir.y != 0)
            AnimateMovement();
        else
            anim.SetLayerWeight(1, 0);
    }

    public void Move()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    public void AnimateMovement()
    {
        anim.SetLayerWeight(1, 1);
        anim.SetFloat("x", dir.x);
        anim.SetFloat("y", dir.y);
        Debug.Log($"{dir.x} : {anim.GetFloat("x")} : {anim.GetLayerWeight(0)}");
    }

    public void ChoseDir() 
    {
        float h = Random.Range(-1f, 1f);
        float v = Random.Range(-1f, 1f);
        Vector2 moveDir = new Vector2(h, v).normalized;

        dir = moveDir;
    }
}
