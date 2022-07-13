using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResearchData
{
    public int id;
    public ResearchInfo info;
    // ���� ������ ���� �� �ְ� �������� Ȯ���ϴ� ����.
    // isPrime == true �� ���, �ش� ������ �°� ��ġ�� ����ȴ�.
    public bool isPrime;

    public ResearchData(ResearchInfo info)
    {
        this.id = info.code;
        this.info = info;
        this.isPrime = false;
    }
}
