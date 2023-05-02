using System.Collections;
using UnityEngine;

public class DduduGemFeedManage : MonoBehaviour
{
    // 뚜두 하위의 Canvas 오브젝트에 붙어있으며, Gem과 Feed와 FeedUI에 기능을 준다.
    public FishSelectListGenerator FishSelectList;
    public Ddudu ddudu;
    
    [SerializeField] private Gem _iconGem;

    public void OnClickFeed()
    {
        FishSelectList.gameObject.SetActive(true);
        FishSelectList.selectedDdudu = ddudu;
    }

    public void GemGaCha(int fishCode)  // 보석 확률 계산
    {
        if (!_iconGem.gameObject.activeSelf) 
        {
            StartCoroutine(EarnGem(fishCode));
        }
    }

    IEnumerator EarnGem(int fishCode)
    {
        yield return new WaitForSeconds(2.0f);
        _iconGem.SetEarnGem(fishCode);
    }
}
