using System.Collections;
using UnityEngine;

public class DduduGemFeedManage : MonoBehaviour
{
    // 뚜두 하위의 Canvas 오브젝트에 붙어있으며, Gem과 Feed와 FeedUI에 기능을 준다.
    public FishSelectListGenerator FishSelectList;
    public Ddudu ddudu;
    public GameObject IconGem;
    public GameObject IconFeed;

    public void OnClickFeed()
    {
        FishSelectList.gameObject.SetActive(true);
        FishSelectList.selectedDdudu = ddudu;
    }

    public void GemGaCha()  // 보석 확률 계산
    {
        if (!IconGem.activeSelf) 
        {
            int ran = Random.Range(1, 100);
            if (ran <= 60) 
            {   // 확률 변수로 바꾸기
                StartCoroutine(EarnGem());
            }
        }
    }

    IEnumerator EarnGem()
    {
        yield return new WaitForSeconds(0.5f);
        IconGem.SetActive(true);
    }
}
