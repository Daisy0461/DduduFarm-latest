using UnityEngine;
using UnityEngine.UI;

public class WorkableList : MonoBehaviour
{// WorkableDdudu
    public Ddudu ddudu;
    public Image BGImg;
    public GameObject dimmed;
    public Text dimmedText;
    public Image iconImg;
    public Text dduduNameTxt;
    public Image buildImg;
    public Image buildBG;
    public Image curSatiety;
    public Image afterSatiety;
    public float useSatiety;

    public void SetValue() {
        iconImg.sprite = Resources.Load<Sprite>(ddudu.data.info.imgPath);
        dduduNameTxt.text = ddudu.data.info.name;
        buildImg.sprite = Resources.Load<Sprite>(BuildingManager.Instance.GetInfo(ddudu.data.interest + (int)DataTable.Craft + 1).imgPath);
        curSatiety.fillAmount = (float)ddudu.data.satiety / (float)ddudu.data.info.maxFull;
        afterSatiety.fillAmount = (float)((ddudu.data.satiety - useSatiety <= 0) ? 0 : ddudu.data.satiety - useSatiety) 
                                / (float)ddudu.data.info.maxFull;
    }

    public void SetSatiety()
    {
        curSatiety.fillAmount = (float)ddudu.data.satiety / (float)ddudu.data.info.maxFull;
        afterSatiety.fillAmount = (float)((ddudu.data.satiety - useSatiety <= 0) ? 0 : ddudu.data.satiety - useSatiety)
                                / (float)ddudu.data.info.maxFull;
    }
}
