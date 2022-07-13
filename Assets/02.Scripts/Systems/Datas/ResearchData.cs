using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResearchData
{
    public int id;
    public ResearchInfo info;
    // 같은 연구의 레벨 중 최고 레벨인지 확인하는 변수.
    // isPrime == true 인 경우, 해당 연구에 맞게 수치가 적용된다.
    public bool isPrime;

    public ResearchData(ResearchInfo info)
    {
        this.id = info.code;
        this.info = info;
        this.isPrime = false;
    }
}
