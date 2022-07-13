using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menubar : MonoBehaviour
{
    public RectTransform menubarRect;
    private bool isMenubarUp;
    public GameObject MenubarQuit;

    public void OnClickMenu()
    {
        if (isMenubarUp == true)
        {
            StartCoroutine(MovingMenubar(-1));
            MenubarQuit.SetActive(false);
        }
        else
        {
            StartCoroutine(MovingMenubar(1));
            MenubarQuit.SetActive(true);
        }
        isMenubarUp = !isMenubarUp;
    }

    const int MOVINGBAR_DOWN_POS = -640;
    const int MOVINGBAR_UP_POS = -40;
    IEnumerator MovingMenubar(int status)
    {
        while (MOVINGBAR_DOWN_POS <= menubarRect.anchoredPosition.y && menubarRect.anchoredPosition.y <= MOVINGBAR_UP_POS)
        {
            menubarRect.transform.position += new Vector3(0, status * 60, 0);
            yield return null;
        }
        if (MOVINGBAR_DOWN_POS > menubarRect.anchoredPosition.y)
        {
            menubarRect.anchoredPosition = new Vector3(menubarRect.anchoredPosition.x, MOVINGBAR_DOWN_POS, 0f);
            MenubarQuit.SetActive(false);
        }
        else if (menubarRect.anchoredPosition.y > MOVINGBAR_UP_POS)
        {
            menubarRect.anchoredPosition = new Vector3(menubarRect.anchoredPosition.x, MOVINGBAR_UP_POS, 0f);
            MenubarQuit.SetActive(true);
        }
    }

    private Vector2 mouseCurPos;
    private float dir;
    public void BeginDrag()
    {
        mouseCurPos = Input.mousePosition;
    }

    public void OnDrag()
    { 
        dir = Input.mousePosition.y - mouseCurPos.y;
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        menubarRect.transform.position += new Vector3(0, dir, 0);
        if (menubarRect.anchoredPosition.y > -40) menubarRect.anchoredPosition = new Vector2(menubarRect.anchoredPosition.x, -40);
        mouseCurPos = Input.mousePosition;
    }
        
    public void EndDrag()
    {
        if (dir < 0)
        {
            StartCoroutine(MovingMenubar(-1));
            isMenubarUp = false;
        }
        else
        {
            StartCoroutine(MovingMenubar(1));
            isMenubarUp = true;    
        }
    }
}
