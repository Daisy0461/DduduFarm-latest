
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FishFarmTalkText : MonoBehaviour
{
    [SerializeField] private Text talkText;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image ownerImg;

    private int randomInt;

    public void Start()
    {
        GreetText();
    }

    public void GreetText()
    {
        talkText.text = "";
        talkText.DOKill();
        ownerImg.sprite = sprites[0];
        randomInt = Random.Range(0, 4);
        
        if(randomInt == 0)
        {
            talkText.DOText("어서오시냥", 0.8f);
        }
        else if (randomInt == 1)
        {
            talkText.DOText("물고기랑 놀러 온거냥?", 1.7f);
        }
        else if (randomInt == 2)
        {
            talkText.DOText("반갑다냥", 0.7f);
        }
        else if (randomInt == 3)
        {
            talkText.DOText("오랜만이냥", 0.8f);
        }
    }

    public void EventText(int eventNum)
    {
        talkText.text = "";
        talkText.DOKill();
        randomInt = Random.Range(0, 2);
        ownerImg.sprite = sprites[randomInt];

        if (eventNum == 1) 
        {// 물고기 알 넣기
            talkText.DOText("애기 물고기다냥", 1.1f);
        } 
        else if (eventNum == 2) 
        {// 같은 양식장 5번 클릭
            talkText.DOText("물고기 괴롭히지 마라냥!", 1.7f);
        } 
        else if (eventNum == 3) 
        {// 물고기 수확
            talkText.DOText("다 자랐다냥", 1.2f);
        }
    }
}
