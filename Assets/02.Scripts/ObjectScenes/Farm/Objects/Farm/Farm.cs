using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    //여기에는 그게 존재해야할 듯
    //드래그 작물이 드래그되어서 오면 작물이 심어지는 그리고 
    //작물이 심어져있는지를 알 수 있는 코드 -> 중복으로 심어지는것 방지     -> Planted
    //어떤 작물이 심어져 있는지? -> 다시 Instatiate or pulling하기 위해서
    public bool isPlanted = false;

    void OnCollisionStay2D(Collision2D coll)
    { 
        //Debug.Log("충돌은 감지함.");
        if(coll.gameObject.tag == "Crop"){
            //Debug.Log("Crop 감지");
            isPlanted = true;
        }
    }

    public void Planted()
    {      
        //일단 2가지 경우만 생각해서 if else로 만듦. - 심기 전, 심은 후, 캐고 난 후 (심기 전)
        if(!isPlanted)
        {
            isPlanted = true;
        }
        else if(isPlanted)
        {
            isPlanted = false;
        }
    }
}