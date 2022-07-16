using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineDduduUI : MonoBehaviour
{
    public GameObject dduduListUI;

    public void ActiveCombineDduduUIEvent()
    {
        dduduListUI.SetActive(true);
    }

    public void InactiveCombineDduduUIEvent()
    {
        dduduListUI.SetActive(false);
    }
}
