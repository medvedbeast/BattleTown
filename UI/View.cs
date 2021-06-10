using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    public GameObject anchor;

    public void Show()
    {
        anchor.SetActive(true);
    }

    public void Hide()
    {
        anchor.SetActive(false);
    }
}
