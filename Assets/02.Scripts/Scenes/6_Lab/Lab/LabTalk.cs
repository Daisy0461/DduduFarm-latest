using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LabTalk: MonoBehaviour
{
    [SerializeField]
    private Text talkText;
    private int randomInt;
    [SerializeField]
    private Sprite[] ownerSprite;
    [SerializeField]
    private Image ownerImage;

    // Start is called before the first frame update
    public void Start(){
        randomInt = Random.Range(0, 4);  //0,1,2,3 중 하나의 값으로 출력.
        if(randomInt == 0){
            //talkText.text = "어서오게나";
            talkText.DOText("새로운 것들을 연구중이야", 1.3f).SetDelay(0.5f);
        }else if (randomInt == 1){
            //talkText.text = "우리 상점에 와줘서 고맙군";
            talkText.DOText("오늘은 뭘 연구해볼까?", 1.3f).SetDelay(0.5f);
        }else if (randomInt == 2){
            //talkText.text = "만나서 반갑세";
            talkText.DOText("뚜두를 만들러 왔어?", 1.3f).SetDelay(0.5f);
        }else if (randomInt == 3){
            //talkText.text = "오랜만이군";
            talkText.DOText("안녕! 뭘 도와줄까?", 1.3f).SetDelay(0.5f);
        }

        if (randomInt % 2 == 0)
        {
            ownerImage.sprite = ownerSprite[0];
        }
        else // (randomInt % 2 == 1)
        {
            ownerImage.sprite = ownerSprite[1];
        }
    }

    public void BuyText(bool refuse=false){
        talkText.text = "";
        randomInt = Random.Range(0, 3);     //3은 포함 X
        // Debug.Log(randomInt);

        if (randomInt == 0){
            //talkText.text = "탁월한 선택이군";
            talkText.DOText("탁월한 선택이군", 1.3f);
        }else if (randomInt == 1){
            //talkText.text = "구매해줘서 고맙소";
            talkText.DOText("구매해줘서 고맙소", 1.3f);
        }else if (randomInt == 2){
            //talkText.text = "더 구매할게 있나?";
            talkText.DOText("더 구매할게 있나?", 1.3f);
        }
        
        if (refuse == true)
        {
            if (randomInt == 0){
                //talkText.text = "탁월한 선택이군";
                talkText.DOText("자네, 돈이 좀 없군", 1.3f);
            }else if (randomInt == 1){
                //talkText.text = "구매해줘서 고맙소";
                talkText.DOText("다른 카드 있는가?", 1.3f);
            }else if (randomInt == 2){
                //talkText.text = "더 구매할게 있나?";
                talkText.DOText("이런, 다음 기회에 오시게", 1.3f);
            }
        }
    } 
}
