using System.Collections.Generic;
using UnityEngine;

public class DduduSpawner : MonoBehaviour
{   // 뚜두 Spawn , 저장, 로드 관리
    DduduManager DM;
    [SerializeField] private UserDataText userDataText;
    public FishSelectListGenerator FishSelectList;
    [HideInInspector] public List<Ddudu> ddudus;    
    
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
        Ddudu newDdudu = SpawnDdudu(id, Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2)));
        newDdudu.transform.SetParent(this.transform);

        var dduduGemFeedManage = newDdudu.GetComponentInChildren<DduduGemFeedManage>();
        dduduGemFeedManage.FishSelectList = FishSelectList;

        if (newDdudu.data.isWork) newDdudu.gameObject.SetActive(false);
        userDataText.RenewText(userDataText.dduduText, DM.GetDataListCount());
        ddudus.Add(newDdudu);
        return newDdudu;
    }

    public static Ddudu SpawnDdudu(int id, Vector3 pos) 
    {
        var data = DduduManager.Instance.GetData(id);
        var index = data.info.code;
        // TODO: 뚜두 생성을 리소스 주소 생성으로 바꾸기
        var newObj = Instantiate(Resources.Load<GameObject>($"{PathAlias.ddudu_prefab_path}{index}"));
        newObj.transform.SetPositionAndRotation(new Vector3(pos.x, pos.y, 0f), Quaternion.Euler(0f, 0f, 0f));
        var newDdudu = newObj.GetComponent<Ddudu>();
        
        newDdudu.data = data;
        if (data.interest == -1) 
        {
            int ran = Random.Range((int)DataTable.Craft, (int)DataTable.Craft + 10);
            newDdudu.data.interest = ran;
        }

        /* TODO: dduduList -> 배열로 바꾸기 */
        Profile.dduduList.Add(index);
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
