using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishInfomationText : MonoBehaviour
{
    [SerializeField]
    private Text fishNameText;
    [SerializeField]
    private Text fishExplainText;
    [SerializeField]
    private Text fishSatietyText;       //FishFile에서 full값
    [SerializeField]
    private Text fishLikeText;          //FishFile에서 like값
    [SerializeField]
    private Image fishImage;
    ItemData iData;
    
    FishManager FM;

    void Start(){
        FM = FishManager.Instance;
    }

    public void GetFishInfomation(int fishKind){
        int fishId = fishKind + (int)DataTable.Fish + 1;
        string fishName = FM.GetInfo(fishId).name;
        string fishExplain = FM.GetInfo(fishId).note;
        int fishSatiety = FM.GetInfo(fishId).full;
        int fishLike = FM.GetInfo(fishId).like;

        fishImage.sprite = Resources.Load<Sprite>(FM.GetInfo(fishId).imgPath2);
        fishNameText.text = fishName;
        fishExplainText.text = fishExplain;
        fishSatietyText.text = fishSatiety.ToString();
        fishLikeText.text = fishLike.ToString();
    }

}
