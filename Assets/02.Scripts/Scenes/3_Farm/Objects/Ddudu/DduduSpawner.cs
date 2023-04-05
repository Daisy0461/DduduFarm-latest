using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SceneSpot {
    Farm,
    Forest
}

public class DduduSpawner : MonoBehaviour
{   // 뚜두 Spawn , 저장, 로드 관리
    [SerializeField] private Text _dduduText;
    [SerializeField] private SceneSpot spot;
    public FishSelectListGenerator FishSelectList;
    [HideInInspector] public List<Ddudu> ddudus;
    DduduManager DM;

    private void OnApplicationFocus(bool focusStatus) 
    {  
        if (focusStatus == false) 
        {
            DM.Save();
        }
    }

    private void Start() 
    {
        DM = DduduManager.Instance;
        LoadDdudu();
        
        // { Test
        //if (this.transform.childCount < 5)
        //{
        //    for(int i=1; i<=6; i++)
        //    {
        //        var id = DM.AddData((int)DataTable.Ddudu+i);
        //        Debug.Log(id);
        //        SpawnDdudu(id);
        //    }
        //} 
        // } Test
    }

    public void LoadDdudu() 
    {
        foreach (var data in DM.GetDataList())
        {
            Ddudu newDdudu = SpawnDdudu(data.id);
            newDdudu.IconGem.SetActive(data.isGemIconActive);
            newDdudu.transform.position = new Vector3(data.x, data.y, data.z);
        }
    }
    
#if UNITY_EDITOR
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            int ran = Random.Range(101, 111);
            var newDdudu = SpawnDdudu(DM.AddData(ran));
        }
    }
#endif // UNITY_EDITOR

    public Ddudu SpawnDdudu(int id)
    {
        var tmpPos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0));
        var pos = new Vector3(tmpPos.x, tmpPos.y, 0);
        var newDdudu = DduduManager.SpawnDdudu(id, pos);
        newDdudu.transform.SetParent(this.transform);

        if (FishSelectList != null)
        {
            var dduduGemFeedManage = newDdudu.GetComponentInChildren<DduduGemFeedManage>();
            dduduGemFeedManage.FishSelectList = FishSelectList;
        }

        if (newDdudu.data.isWork) newDdudu.gameObject.SetActive(false);
        ddudus.Add(newDdudu);

        UpdateDduduText();
        return newDdudu;
    }

    private void UpdateDduduText()
    {
        if (_dduduText != null)
        {
            switch (spot)
            {
            case SceneSpot.Farm: 
                _dduduText.text = DM.GetDataListCount().IntToPrice();
                break;
            case SceneSpot.Forest:
                _dduduText.text = TextFormat.IntToFraction(DM.GetDataListCount(), DM.MaxDduduCount);
                if (DM.GetWorkDduduCount() > 0)
                {
                    _dduduText.text += $"\n<size=25>{DM.GetWorkDduduCount()} 뚜두 다른 일 하는 중</size>";
                }
                break;
            }
        }
    }

    public Ddudu FindDduduObject(int dataId)
    {
        for(int i=0; i < ddudus.Count; i++)
        {
            if (ddudus[i].data.id == dataId)
                return ddudus[i];
        }
        return null;
    }
}
