using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LabSaveData
{
    // ���⵵ ���� �׸� ��ġ �߰�.
    public int farmTimeDecrease = 0;

    // ���� �׸� ���� ��ġ�� ���� �޼��� �ٸ� ��ũ��Ʈ���� ó��. ����� ���常
    // LabSaveData �޼��� ���ο� ���� �׸� ���� ��ġ �Ҵ�.

    // Start is called before the first frame update
    public LabSaveData(LabData data){
        farmTimeDecrease = data.farmTimeDecrease;
    }
}
