using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    private float r;
    public float rotateTime = 0.5f;
    Vector2 dir;

    protected void Start()
    {
        InvokeRepeating("ChoseDir", 0f, 3f);        // 일정 시간마다 랜덤한 이동 방향을 제시
		
        // base.Start();
    }

    public void ChoseDir()
    {
        // 랜덤한 방향으로 움직인다.
        r = UnityEngine.Random.Range(0.0f, 360.0f);
        Vector2 moveDir = new Vector2(0f, 1.0f).normalized;     
        
        dir = moveDir;
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0, 0, r), rotateTime);
    }
}
