using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchCompleteUI : DefaultPanel
{
	public override void Activate(params object[] objs)
	{
		var researchId = (int)objs[0];

		var researchInfo = ResearchManager.Instance.GetInfo(researchId);
		if (researchInfo == null)
		{
			Debug.LogError("researchInfo is Null");
			return;
		}

		_titleText[0].text = researchInfo.name;
		_text[0].text = researchInfo.note;
		_text[1].text = $"{researchInfo.level}";
		_text[2].text = $"{researchInfo.researchValue}";
		_images[0].sprite = Resources.Load<Sprite>(researchInfo.imgPath);

		base.Activate(objs);
	}
}
