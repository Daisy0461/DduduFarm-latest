using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallDduduSlotUI : MonoBehaviour
{
    public struct Coordinate
    {
        private float posX;
        private float posY;

        public Coordinate(float posX, float posY)
        {
            this.posX = posX;
            this.posY = posY;
        }

        public float getX()
        {
            return posX;
        }

        public float getY()
        {
            return posY;
        }
    }

    public GameObject DduduImage;

    public GameObject[] requireMaterialSlotList = new GameObject[3];

    public DduduUnit dduduUnit;
    public List<Coordinate>[] researchMaterialCoordinateList = new List<Coordinate>[3];

    [SerializeField]
    private bool canCall;
    private UserDataText userDataText;
    ItemManager IM;
    ResearchManager RM;

    private void Awake()
    {
        researchMaterialCoordinateList[0] = new List<Coordinate>() { new Coordinate(0, -42) };
        researchMaterialCoordinateList[1] = new List<Coordinate>() { new Coordinate(-70, -42), new Coordinate(70, -42) };
        researchMaterialCoordinateList[2] = new List<Coordinate>() { new Coordinate(-100, -42), new Coordinate(0, -42), new Coordinate(100, -42) };
    }

    private void Start() 
    {
        userDataText = FindObjectOfType<UserDataText>(); 
    }

    // 재료 개수에 따른 위치 설정
    private void OnEnable()
    {
        IM = ItemManager.Instance;
        RM = ResearchManager.Instance;  

        dduduUnit = gameObject.GetComponent<DduduUnit>();

        SetCallDduduImage();
        SetCallMaterialObject();
    }

    public void SetCallDduduImage()
    {
        DduduImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(dduduUnit.callDduduImagePath);
    }

    public void SetCallMaterialObject() // 재료 위치 잡고 재료 개수 체크 후 색깔 변경
    {
        // canCall = ResearchManager.Instance.GetDataList().Contains(ResearchManager.Instance.GetData(dduduUnit.requireResearchID));

        int requireMaterialListCount = dduduUnit.requireMaterialList.Count - 1;
        int requireMaterialIndex = 0;

        foreach (Coordinate matCoordinate in researchMaterialCoordinateList[requireMaterialListCount])
        {
            GameObject requireMaterial = requireMaterialSlotList[requireMaterialIndex];
            string requireMaterialText = "";

            requireMaterial.SetActive(true);

            requireMaterial.transform.SetParent(gameObject.transform.Find("Call Require Material Slots"));
            requireMaterial.GetComponent<RectTransform>().anchoredPosition = new Vector3(matCoordinate.getX(), matCoordinate.getY(), 0);

            requireMaterial.transform.Find("Call Material Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(IM.GetInfo(dduduUnit.requireMaterialList[requireMaterialIndex].requireMaterialId).imgPath);
            
            int requireMaterialAmount = 0;
            if (IM.IsDataExist(dduduUnit.requireMaterialList[requireMaterialIndex].requireMaterialId))
                requireMaterialAmount = IM.GetData(dduduUnit.requireMaterialList[requireMaterialIndex].requireMaterialId).amount;
            requireMaterialText = requireMaterialAmount.ToString() + "/" + dduduUnit.requireMaterialList[requireMaterialIndex].requireMaterialCount.ToString();

            requireMaterial.transform.Find("Call Material Text").GetComponent<Text>().text = requireMaterialText;

            if (requireMaterialAmount >= dduduUnit.requireMaterialList[requireMaterialIndex].requireMaterialCount)
            {
                requireMaterial.transform.Find("Call Material Text").GetComponent<Text>().color = Color.green;
            }
            else
            {
                requireMaterial.transform.Find("Call Material Text").GetComponent<Text>().color = Color.red;
                canCall = false;
            }

            requireMaterialIndex++;
        }
    }

    public void UpdateCallMaterialItem()
    {
        foreach (DduduUnit.RequireMaterial requireMaterial in dduduUnit.requireMaterialList)
        {
            IM.RemoveData(requireMaterial.requireMaterialId, requireMaterial.requireMaterialCount);
        }
    }

    public void CallDduduEvent()
    {
        // TestTempStatus();

        // if(canCall)
        // {
        //     UpdateCallMaterialItem();
        //     OrderAddDduduFromOtherScene();

        //     SetCallMaterialObject();
        // }

        // 뚜두를 부를 수 있는지 체크 후 부를 수 있다면 부르기
        // canCall:
        canCall = CheckMaterials();
        Debug.Log(canCall);
        if(canCall)
        {
            UpdateCallMaterialItem();       // 재료 차감
            OrderAddDduduFromOtherScene();  // 뚜두 추가

            SetCallMaterialObject();        // 재료 색 변경
        }
        TestTempStatus();
    }

    private bool CheckMaterials()
    {
        bool ret = true;
        // 선행 연구 체크

        // 재료 체크
        for (int i=0; i<dduduUnit.requireMaterialList.Count; i++)
        {
            if (IM.IsDataExist(dduduUnit.requireMaterialList[i].requireMaterialId))
            {
                if (IM.GetData(dduduUnit.requireMaterialList[i].requireMaterialId).amount < dduduUnit.requireMaterialList[i].requireMaterialCount)
                    ret = false;
            } else ret = false;
        }
        return ret;
    }

    private void OrderAddDduduFromOtherScene()
    {
        DduduManager.Instance.AddData(dduduUnit.dduduID);
        userDataText.RenewText(userDataText.dduduText, DduduManager.Instance.GetDataListCount());   // 뚜두 텍스트 변경

        Debug.Log("뚜두 추가 완료");
    }

    private void TestTempStatus()
    {
        if (!RM.GetDataList().Contains(RM.GetData(dduduUnit.requireResearchID)))
            Debug.Log("연구가 진행되지 않았습니다.");

        // else if ()   // 재료 확인
            Debug.Log("재료가 부족합니다.");
    }
}
