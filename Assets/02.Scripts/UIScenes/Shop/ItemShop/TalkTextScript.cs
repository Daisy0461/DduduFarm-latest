using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TalkTextScript : MonoBehaviour
{
    public Text talkText;
    private int randomInt;

    public Sprite originSprite;
    public Sprite talkSprite;
    public Image ownerImg;

    private bool isGreet;

    public void OnEnable(){
        GreetText();
    }

    public void GreetText()
    {
        if (isGreet == true) return;

        talkText.text = "";
        talkText.DOKill();
        ownerImg.sprite = originSprite;
        randomInt = Random.Range(0, 4);  //0,1,2,3 중 하나의 값으로 출력.
        
        if(randomInt == 0){
            talkText.DOText("어서오게나", 0.5f);
        }else if (randomInt == 1){
            talkText.DOText("우리 상점에 와줘서 고맙군", 1.4f);
        }else if (randomInt == 2){
            talkText.DOText("만나서 반갑세", 0.7f);
        }else if (randomInt == 3){
            talkText.DOText("오랜만이군", 0.5f);
        }
        isGreet = true;
    }

    public void BuyText(bool refuse=false){
        ownerImg.sprite = talkSprite;
        talkText.text = "";
        talkText.DOKill();
        randomInt = Random.Range(0, 3);     //3은 포함 X

        if (randomInt == 0){
            talkText.DOText("탁월한 선택이군", 0.8f);
        }else if (randomInt == 1){
            talkText.DOText("구매해줘서 고맙소", 0.9f);
        }else if (randomInt == 2){
            talkText.DOText("더 구매할게 있나?", 1.0f);
        }
        
        if (refuse == true)
        {
            if (randomInt == 0){
                talkText.DOText("자네, 돈이 좀 없군", 1.1f);
            }else if (randomInt == 1){
                talkText.DOText("다른 카드 있는가?", 1.0f);
            }else if (randomInt == 2){
                talkText.DOText("이런, 다음 기회에 오시게", 1.4f);
            }
        }
    } 

    public void UnableToBuyText(string content)
    {
        talkText.text = "";
        talkText.DOKill();
        talkText.DOText(content, content.Length * 0.1f); 
        isGreet = false;
    }
}
