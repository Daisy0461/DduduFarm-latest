using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupBuilding : MonoBehaviour
{
	ItemManager IM;

	[Header("now Selected")]
	public Common common;
	public Craft craft;
	[SerializeField] private GameObject buildings;    // 건물 부모
	[SerializeField] private List<Common> commons;
	[SerializeField] private List<Craft> crafts;
	
	[Header("Ddudu List")]
	[SerializeField] private DduduSpawner dduduSpawner;
	[SerializeField] private List<Ddudu> ddudus;
	[SerializeField] private List<WorkableList> workableLists; 
	[SerializeField] private Transform contentParent;
	private GameObject[] contentChildren;
	public int index;

	[Header("both")]
	public Text nameText;
	public Image iconImg;
	public AudioSource audioSource;

	[Header("common")]
	public Text explainText;
	public Text goldCycleTxt;
	public Text goldStateTxt;
	public Text goldBtnTxt;

	[Header("Craft")]
	public WorkableList workDdudu;
	public Image matImg;
	public Text matText;
	public Image outputImg;
	public Text outputText;
	public Text craftCycleTxt;
	public Text satietyTxt;
	public GameObject workableDduduPrefab;
	public Text btnTxt;
	public Image btnImg;

	public GameObject panel_WorkingInfo;
	public Image img_Ddudu;
	public Text txt_WorkingTime;
	public Text txt_Output;
	bool working;

	private void Start()
	{
		CreateDduduList();
		SortDduduList();
		SetValue();
	}

	private void OnEnable()
	{
		if (IM == null) IM = ItemManager.Instance;
		if (common != null)
		{
			commons = new List<Common>(buildings.GetComponentsInChildren<Common>());
			index = commons.IndexOf(common);
		}
		else
		{
			if (workDdudu != null)
			{
				workDdudu.BGImg.color = new Color(1, 0.7783019f, 0.7783019f);
				workDdudu = null;
			}
			crafts = new List<Craft>(buildings.GetComponentsInChildren<Craft>());
			ddudus = new List<Ddudu>(dduduSpawner.ddudus);
			index = crafts.IndexOf(craft);
			SortDduduList();
			SetValue();
			RenewWorkBtn();
		}
	}

	private void Update()
	{
		if (common != null) // 일반건물일 때
		{
			goldStateTxt.text = common.remainTimeStr;
			if (common.data.cycleRemainTime <= 0f)
			{
				goldStateTxt.text = "생산된 골드 : " + common.data.info.outputAmount;
				goldBtnTxt.text = "획득하기";
			}
		}
		else if (craft != null)
		{
			RenewWorkBtn();
			if (working)
			{
				txt_WorkingTime.text = craft.remainTimeStr;
			}
		}
	}

	public void CreateDduduList()
	{
		foreach (var ddudu in ddudus)
		{
			WorkableList newList = Instantiate(workableDduduPrefab, contentParent).GetComponent<WorkableList>();
			newList.ddudu = ddudu;
			newList.GetComponent<Button>().onClick.AddListener(() => OnclickDduduBtn(newList));
			workableLists.Add(newList);
		}
	}

	public void SortDduduList()
	{// 작업 가능한 뚜두(관심 건물인 뚜두-일반 뚜두)-작업 불가능한 뚜두 순으로 정렬 + 딤드
		for (int i = 0; i < workableLists.Count; i++)
		{
			if (workableLists[i].ddudu.data.isWork)
				workableLists[i].dimmedText.text = "다른 곳에서 일하는 중이야";
			else if (workableLists[i].ddudu.data.satiety < craft.data.info.requireFull)
				workableLists[i].dimmedText.text = "포만도가 모자라";
			else
			{
				workableLists[i].dimmed.SetActive(false);
				workableLists[i].GetComponent<Button>().enabled = true;
				continue;
			}
			workableLists[i].dimmed.SetActive(true);
			workableLists[i].GetComponent<Button>().enabled = false;
		}
		workableLists.Sort(delegate(WorkableList a, WorkableList b) 
		{
			if (a.ddudu.data.satiety < craft.data.info.requireFull || b.ddudu.data.satiety < craft.data.info.requireFull)
				return a.ddudu.data.satiety < craft.data.info.requireFull ? a.ddudu.data.satiety*100 : -1;
			return a.ddudu.data.satiety.CompareTo(b.ddudu.data.satiety);
		});
		workableLists.Sort(delegate(WorkableList a, WorkableList b)
		{
			if (a.ddudu.data.isWork || b.ddudu.data.isWork)
				return a.ddudu.data.isWork ? int.MaxValue : -1;	// a가 working이면 순서 변경 b가 working이면 그대로
			return 0;
		});
		workableLists.Sort(delegate(WorkableList a, WorkableList b)
		{
			if (a.ddudu.data.interest + (int)DataTable.Craft + 1 == craft.data.info.code && a.ddudu.data.satiety >= craft.data.info.requireFull) 
				return int.MinValue;
			return 0;
		});	
		for (int i = 0; i < workableLists.Count; i++)
            workableLists[i].transform.SetSiblingIndex(i);
	}

	void SetValue()
	{
		foreach (var list in workableLists)
		{
			list.useSatiety = craft.data.info.requireFull;
			if (list.ddudu.data.interest + (int)DataTable.Craft + 1 != craft.data.info.code)
				list.buildBG.color = new Color(1, 0.8632076f, 0.8632076f);
			else
			{
				list.buildBG.color = new Color(0.9681532f, 1, 0);
				list.BGImg.color = new Color(1, 0.7783019f, 0.7783019f);
			}
			list.SetValue();
		}
	}

	public void OnclickDduduBtn(WorkableList newList)
	{
		if (workDdudu != null)
		{
			workDdudu.BGImg.color = new Color(1, 0.7783019f, 0.7783019f);
			if (workDdudu == newList)
			{
				workDdudu = null;
				return;
			}
		}
		workDdudu = newList;
		workDdudu.BGImg.color = new Color(0.9926031f, 1, 0.7028302f);
	}

	public void OnClickArrow(bool isRight)
	{
		if (common != null) // 일반 건물
		{
			index = isRight ? (++index) % commons.Count : (--index) % commons.Count;
			if (index < 0) index = commons.Count - 1;
		}
		else                // 공방 건물
		{
			index = isRight ? (++index) % crafts.Count : (--index) % crafts.Count;
			if (index < 0) index = crafts.Count - 1;
			craft = crafts[index];
			SortDduduList();
			SetValue();
			RenewWorkBtn();
		}
		RenewPanel(index);
	}

	public void RenewPanel(int index)
	{
		if (common != null) // 일반 건물
		{
			common = commons[index];

			nameText.text = common.data.info.name;
			iconImg.sprite = Resources.Load<Sprite>(common.data.info.imgPath);
			explainText.text = common.data.info.note;
			if (common.data.cycleRemainTime <= 0f)
			{
				goldStateTxt.text = "생산된 골드 : " + common.data.info.outputAmount;
				goldBtnTxt.text = "획득하기";
			}
			else
			{
				goldCycleTxt.text = "골드 생산 시간 : " + (common.data.info.cycleTime).Sec2Time();
				goldBtnTxt.text = "확인";
			}
		}
		else                // 공방 건물
		{
			craft = crafts[index];

			RenewWorkBtn();
			if (working)
			{
				panel_WorkingInfo.SetActive(true);
				if (craft.data.workerId != 0)
					img_Ddudu.sprite = Resources.Load<Sprite>(DduduManager.Instance.GetData(craft.data.workerId).info.imgPath);
				txt_WorkingTime.text = craft.remainTimeStr;
				txt_Output.text = "예상 획득량 : " + IM.GetInfo(craft.data.info.outputId).name + " " + craft.data.info.outputAmount + "개";
			}
			else panel_WorkingInfo.SetActive(false);
			nameText.text = craft.data.info.name;
			iconImg.sprite = Resources.Load<Sprite>(craft.data.info.imgPath);
			matImg.sprite = Resources.Load<Sprite>(IM.GetInfo(craft.data.info.matId).imgPath);
			if (!IM.IsDataExist(craft.data.info.matId))
				matText.text = "<color=red>"+0+"</color> / "+craft.data.info.matAmount;
			else if (IM.GetData(craft.data.info.matId)?.amount < craft.data.info.matAmount)
				matText.text = "<color=red>"+IM.GetData(craft.data.info.matId).amount+"</color> / "+craft.data.info.matAmount;
			else
				matText.text = IM.GetData(craft.data.info.matId).amount +" / "+ craft.data.info.matAmount;
			outputImg.sprite = Resources.Load<Sprite>(IM.GetInfo(craft.data.info.outputId).imgPath);
			outputText.text = IM.GetInfo(craft.data.info.outputId).name + " " + craft.data.info.outputAmount;
			craftCycleTxt.text = "작업 시간 : " + (craft.data.info.cycleTime).Sec2Time();
			satietyTxt.text = "포만도 소모량 : " + craft.data.info.requireFull;
		}
	}

	public void RenewWorkBtn()
	{
		working = false;
		if (!craft.IsWorking())	// Idle
		{
			btnTxt.text = "작업하기";
			btnImg.color = Color.gray;
			if (IM.IsDataExist(craft.data.info.matId))
			{
				if (IM.GetData(craft.data.info.matId).amount >= craft.data.info.matAmount &&
					workDdudu != null)	// 재료 확인, 뚜두 선택 확인
				{
					btnImg.color = Color.white;
					working = true;
				}
			}
		}
		else	// working
		{
			btnImg.color = Color.white;
			working = true;
			if (!craft.data.isDone)
				btnTxt.text = "확인";
			else
				btnTxt.text = "획득하기";
		}
	}

	public void OnClickConfirm()
	{// 확인 버튼의 기능
		if (common.data.cycleRemainTime <= 0f)
		{// 획득하기
			common.OnClickGoldBtn();
		}
		else audioSource.Play();
		this.gameObject.SetActive(false);
	}

	public void OnClickWork()	// 버튼 클릭
	{
		if (!working)
		{
			// 에러팝업 띄우기 - 기존의 색 변경 제거
			var Err = GameObject.FindGameObjectWithTag("Error")?.transform;
            if (Err != null)
            {
                Err.GetChild(0).gameObject.SetActive(true);
                Err.GetChild(1).GetComponent<TextObject>().contentText.text = "작업을 할 수 없습니다.";
                Err.GetChild(1).gameObject.SetActive(true);
            }
			return;
		}
		else if (!craft.IsWorking()) // 작업하기
		{
			IM.RemoveData(craft.data.info.matId, craft.data.info.matAmount);
			craft.data.workerId = workDdudu.ddudu.data.id;
			DduduManager.Instance.GetData(craft.data.workerId).isWork = true;
			workDdudu.ddudu.gameObject.SetActive(false);
			craft.SetRechargeScheduler(true);
		}
		else // 작업 중 + 작업 완료 시
		{
			if (craft.data.isDone)// 작업 완료 - 획득하기
			{
				craft.OnClickOutput();
			}
		}
		this.gameObject.SetActive(false);
	}
}
