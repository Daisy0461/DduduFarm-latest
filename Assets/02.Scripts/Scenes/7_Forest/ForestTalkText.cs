
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ForestTalkText : MonoBehaviour
{
    public Text talkText;
    public Image ownerImg;
    public Sprite[] sprites;
    public string[] talkRepertories;

    private int randomInt;

    public void Start(){
        GreetText();
    }

    public void GreetText()
    {
        talkText.text = "";
        talkText.DOKill();
        ownerImg.sprite = sprites[0];
        randomInt = Random.Range(0, talkRepertories.Length);  //0,1,2,3 중 하나의 값으로 출력.
        
        talkText.DOText(talkRepertories[randomInt], talkRepertories[randomInt].Length * 0.1f + 0.3f);
    }

    public void EventText(int eventNum)
    {
        talkText.text = "";
        talkText.DOKill();
        randomInt = Random.Range(0, 2);
        ownerImg.sprite = sprites[randomInt];

        if (eventNum == 1) {
            // 물고기 알 넣기
            talkText.DOText("애기 물고기다냥", 1.1f);
        } else if (eventNum == 2) {
            // 같은 양식장 5번 클릭
            talkText.DOText("물고기 괴롭히지 마라냥!", 1.7f);
        } else if (eventNum == 3) {
            // 물고기 수확
            talkText.DOText("다 자랐다냥...", 1.2f);
        }
    }
}
