using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEggMovement : MonoBehaviour
{
    [SerializeField]
    private bool isDown = true;
    public float rotateTime = 0.5f;
    private Vector2 moveDir; 
    Vector2 dir;

    protected void Start()
    {
        InvokeRepeating("ChoseMove", 0f, 1.0f);        // 일정 시간마다 랜덤한 이동 방향을 제시
    }

    public void ChoseMove()
    {
        // 랜덤한 방향으로 움직인다.
        //r = UnityEngine.Random.Range(0.0f, 360.0f);
        if(isDown){
            moveDir = new Vector2(0f, 1.0f).normalized; 
            isDown = false;
        }else{
            moveDir = new Vector2(0f, -1.0f).normalized; 
            isDown = true;
        }     
        
        dir = moveDir;
    }

    // void Update()
    // {
    //     transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(0, 0, r), rotateTime);
    // }
}
