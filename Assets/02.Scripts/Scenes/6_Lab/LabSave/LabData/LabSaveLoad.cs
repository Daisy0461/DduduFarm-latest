using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LabSaveLoad : MonoBehaviour
{
    public void SaveLabData(){
        LabDataSaveManager.Save(GameObject.FindObjectOfType<LabData>());       
    }

    public void LoadLabData(){
        AllLabData save = LabDataSaveManager.Load();
        //태그 해줘야함! - 어려운거 아니니까 까먹지 말쟈~!
        GameObject lab = GameObject.FindGameObjectWithTag("LabData");
        LabData labData = lab.GetComponent<LabData>();

        // 위에는 건들면 안 돌아갈 듯. 손 대지 말 것.
        // LabData 처럼 추가

        //여기 값 추가를 위해서는 1단계: LabData에 존재하는 public 변수여야 한다.  2단계: LabSaveData에 지정이 되어있어야한다  3단계: 여기 밑에 다음과 같이 적는다.
        labData.farmTimeDecrease = save.labDatas.farmTimeDecrease;      //farmTimeDecrease 불러오기 밑에 쭈르륵 쓰면 됌
    }
}