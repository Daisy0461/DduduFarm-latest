using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TalkTextScriptForSell : MonoBehaviour
{
    public Text talkText;
    private int randomInt;

    public Sprite originSprite;
    public Sprite talkSprite;
    public Image ownerImg;

    public void OnEnable()
    {   
        talkText.text = "";
        talkText.DOKill();
        ownerImg.sprite = originSprite;
        talkText.DOText("무슨 아이템을 판매하고 싶은가?", 1.7f);
    }

    public void BuyText(){
        ownerImg.sprite = talkSprite;
        talkText.text = "";
        talkText.DOKill();
        randomInt = Random.Range(0, 3);     //3은 포함 X

        if (randomInt == 0){
            talkText.DOText("탁월한 선택이군", 0.8f);
        }else if (randomInt == 1){
            talkText.DOText("좋은 거래였네", 0.7f);
        }else if (randomInt == 2){
            talkText.DOText("더 판매할게 있나?", 1.0f);
        }
        
    } 
}
