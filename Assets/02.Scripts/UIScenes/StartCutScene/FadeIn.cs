using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image Panel;
    float time = 0;
    float F_time = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInSystem());
    }

    IEnumerator FadeInSystem()
    {
        Panel.gameObject.SetActive(true);
        Color alpha = Panel.color;

        while(alpha.a> 0.0f){
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }

        Panel.gameObject.SetActive(false);
        yield return null;
    }
}
