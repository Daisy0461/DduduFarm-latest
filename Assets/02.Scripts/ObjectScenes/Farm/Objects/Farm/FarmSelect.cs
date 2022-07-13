using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmSelect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // 이 스크립트에서는 꾹 눌렀을 때 UI 출현, 밭 색깔 바꾸기까지만 되도록 만들어야함.
    [SerializeField]
    private GameObject corpListUI_ScrollView;       //UI를 넣어두는 GameObject.
    [SerializeField]
    private FarmFind farmFind;

    public void OnPointerDown(PointerEventData eventData) {         
        StartCoroutine("PressCheck");
    }

    public void OnPointerUp(PointerEventData eventData) {
        StopCoroutine("PressCheck");
    }

    IEnumerator PressCheck(){
        yield return new WaitForSeconds(0.3f);

        corpListUI_ScrollView.SetActive(true);

        for(int i=0; i<farmFind.farmStateList.Length; i++){
            //리스트를 3개만 넣으니까 거기 있는 farm만 색이 변하고 다른 밭은 변하지 않는다 이를 해결해야겠다.
            farmFind.farmStateList[i].ChangeFarmColor();
        }
    }
}