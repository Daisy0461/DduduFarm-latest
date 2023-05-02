using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarnAnim : MonoBehaviour
{
    [SerializeField] private TextMeshPro _tmpText;

    private void OnEnable() 
    {
        StartCoroutine(Anim());
    }

    public void SetText(string content)
    {
        _tmpText.text = content;
    }

    IEnumerator Anim()
    {
        float time = 0f;
        while (time < 0.8f)
        {
            this.transform.position += new Vector3(0, 0.05f, 0);
            yield return new WaitForSeconds(0.05f);
            time += 0.05f;
        }
        this.transform.position -= new Vector3(0, time, 0); 
        this.gameObject.SetActive(false);
    }
}
