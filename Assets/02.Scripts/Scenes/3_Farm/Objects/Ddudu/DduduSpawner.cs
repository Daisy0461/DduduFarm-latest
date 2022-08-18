using System.Collections.Generic;
using UnityEngine;

public class DduduSpawner : MonoBehaviour
{   // 뚜두 Spawn , 저장, 로드 관리
    DduduManager DM;
    [SerializeField] private UserDataText userDataText;

    [Header("Spawn Ddudu")] // 뚜두 관리
    [SerializeField] private GameObject[] DduduPrefab;
    [SerializeField] private Craft[] Crafts;
    [HideInInspector] public List<Ddudu> ddudus;
    public FishSelectListGenerator FishSelectList;
    
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
        // 연구소에서 뚜두 추가하기 위해 만든 메서드.
        AddWaitingCreatedDduduList(); 

        //Test
        if (this.transform.childCount < 5)
        for(int i=1; i<=5; i++)
        {
            var id = DM.AddData((int)DataTable.Ddudu+i);
            var newDdudu = SpawnDdudu(id);
        } 
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
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            int ran = Random.Range(101, 111);
            var newDdudu = SpawnDdudu(DM.AddData(ran));
        }
    }

    public void AddWaitingCreatedDduduList()
    {
        // foreach(DduduInfo waitingCreatedDdudu in DduduManager.Instance.WaitingCreatedDduduInfoList)
        // {
        //     SpawnDdudu(waitingCreatedDdudu.id - (int)DataTable.Ddudu);
        // }
    }

    public Ddudu SpawnDdudu(int id) 
    {
        var data = DM.GetData(id);
        var index = data.info.code - (int)DataTable.Ddudu - 1;
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2));
        var newObj = Instantiate(DduduPrefab[index], new Vector3(pos.x, pos.y, 0f), Quaternion.Euler(0f, 0f, 0f));
        var newDdudu = newObj.GetComponent<Ddudu>();
        var dduduGemFeedManage = newDdudu.GetComponentInChildren<DduduGemFeedManage>();
        
        newDdudu.data = data;
        if (data.interest == -1 && Crafts.Length > 0) 
        {
            int ran = Random.Range(0, Crafts.Length);
            newDdudu.interest = Crafts[ran];
            newDdudu.data.interest = ran;
        }
        else 
        {
            newDdudu.interest = Crafts[data.interest];
        }
        dduduGemFeedManage.FishSelectList = FishSelectList;
        newDdudu.transform.parent = this.gameObject.transform;

        /* TODO: dduduList -> 배열로 바꾸기 */
        Profile.dduduList.Add(index);
        userDataText.RenewText(userDataText.dduduText, DM.GetDataListCount());
        if (data.isWork) newDdudu.gameObject.SetActive(false);
        ddudus.Add(newDdudu);
        return newDdudu;
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
