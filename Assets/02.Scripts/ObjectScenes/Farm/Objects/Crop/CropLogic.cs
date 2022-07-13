using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//실시간이 된다면 Awake에 값을 받아와야할거 같음.

public class CropLogic : MonoBehaviour
{
    public float maxGrowth = 100f;
    public float growthLevel = 0;

    private CropState cropState;
    private CropGrowTime cropGrowTime;
    private Vector3 originPos;

    private float getRemainGrowTime;
    private float getTimeMaxGrowInterval;

    void Start()
    {
        originPos = gameObject.GetComponent<CropGrowTime>().originPos;          //0.1, 0.25 다 되어 있는 값.
        cropState = gameObject.GetComponent<CropState>();
        cropGrowTime = gameObject.GetComponent<CropGrowTime>();
        growthLevel = 0;
    }
    
    void Update()
    {
        //RemainGrowTime과 MaxGrowInterval을 가져온다.
        getRemainGrowTime = cropGrowTime.getRemainGrowTime();
        getTimeMaxGrowInterval = cropGrowTime.getTimeMaxGrowInterval();

        if(getRemainGrowTime <= 0){
            growthLevelMakeMax();
        }
        
        if(growthLevel >= maxGrowth){ 
            //Debug.Log("growthLevel이 max를 넘김 즉 Scale 만땅"); 계속 된다 왜일까?
            ScaleCrop(0.0f);
        }else{
            //작물이 점점 커지는 것.
            getRemainGrowTime = cropGrowTime.getRemainGrowTime();
            getTimeMaxGrowInterval = cropGrowTime.getTimeMaxGrowInterval();
            float getResult = getRemainGrowTime / getTimeMaxGrowInterval;  //초마다 커지는것, getResult의 값은 0.9에서 0.1로 간다.
            //Debug.Log("계산값 : " + getRemainGrowTime / getTimeMaxGrowInterval);  

            if(getResult >= 0.6){
                ScaleCrop(0.6f);        //최소크기 설정
            }else{      
                ScaleCrop(getResult);
            }
        }

    }

    void ScaleCrop(float getResult)        //Crop의 크기를 늘리는 코드임. 잘 크는데 대신에 조금 뚝뚝 커짐.
    {
        growthLevel = maxGrowth - (maxGrowth * getResult);          //원래 여기 maxGrowth = 100; 이건 시간이 지나면서 점진적으로 1로 바뀜
        Vector3 cropScale = new Vector3(0.5f, 0.5f, 0.5f) * (growthLevel /maxGrowth);       //점점 커져서 0.5, 0.5, 0.5에 도달
        gameObject.transform.localScale = cropScale;

        //여기 밑에는 위치 바꾸는 작업을 함.
        if(growthLevel/maxGrowth != 1){
            Vector3 movePos = new Vector3(0.1f, 0.25f, 0) * (1-(growthLevel/maxGrowth));    //점점 작아져서 0, 0, 0까지 도달
            gameObject.transform.position = originPos - movePos;
        }
        else{
            gameObject.transform.position = originPos;
        }
    }

    public void growthLevelMakeMax(){
        growthLevel = maxGrowth;
    }

}
