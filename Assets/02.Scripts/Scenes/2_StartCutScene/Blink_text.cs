using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink_text : MonoBehaviour
{
    Text blink_text;
    Color textColor;
    bool overCheck = true;

    void Start()
    {
        blink_text = gameObject.GetComponent<Text>();
        textColor = blink_text.color;
    }

    void Update()
    {
        if (textColor.a < 1.0f && overCheck == false)
        {
            Blink_a_Up();
            if (textColor.a >= 1.0f)
            {
                overCheck = true;
            }
        }
        else if (textColor.a >= 1.0f || overCheck == true)
        {      //결국 1.0까지 도달하고 줄여야하는데 여기만 계속 시도하게 해야한다.
            Blink_a_Down();
            if (textColor.a < 0.2f)
            {
                overCheck = false;
            }
        }
    }

    void Blink_a_Up()
    {
        textColor.a += 0.008f;
        blink_text.color = textColor;
    }

    void Blink_a_Down(){
        textColor.a -= 0.008f;
        blink_text.color = textColor;
    }
}
