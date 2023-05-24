using UnityEngine;
using UnityEngine.UI;

public class FishInfomationText : MonoBehaviour
{
    [SerializeField] private Text fishNameText;
    [SerializeField] private Text fishExplainText;
    [SerializeField] private Text fishSatietyText;       //FishFile에서 full값
    [SerializeField] private Text fishLikeText;          //FishFile에서 like값
    [SerializeField] private Image fishImage;

    public void GetFishInfomation(int fishKind)
    {
        var fishId = fishKind + (int)DataTable.Fish + 1;
        var fishInfo = FishManager.Instance.GetInfo(fishId);
        
        fishNameText.text = fishInfo.name;
        fishExplainText.text = fishInfo.note;
        fishSatietyText.text = fishInfo.full.ToString();
        fishLikeText.text = fishInfo.like.ToString();
        fishImage.sprite = Resources.Load<Sprite>(fishInfo.imgPath2);
    }
}
