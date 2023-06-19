using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutFaidIn : MonoBehaviour
{
    [SerializeField] private Image cutImage;
    [SerializeField] private SpriteRenderer renderer;
    private Color cutImageColor = new Color(1, 1, 1, 0);

    void OnEnable()
    {
        if (cutImage == null)
        {
            renderer.color = cutImageColor;
        }
        else
        {
            cutImage.color = cutImageColor;
        }
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (cutImageColor.a < 1.0)
        {
            cutImageColor.a = cutImageColor.a + 0.03f;
            if (cutImage != null)
            {
                cutImage.color = cutImageColor;
            }
            else if (renderer != null)
            {
                renderer.color = cutImageColor;
            }
            yield return null;
        }
    }
}
