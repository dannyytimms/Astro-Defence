using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlayController : MonoBehaviour
{
    public static UIOverlayController SharedInstance;

    public Text titleText;

    private void Awake()
    {
        SharedInstance = this;
    }

    public void SetTitle(string title)
    {
       // titleText.text = title;
    }

    public void OnButtonClick()
    {

    }
}
