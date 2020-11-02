using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRectHandler : MonoBehaviour
{
    RectTransform commonRoot, thisRect;

    public GameObject titleBarPrefab, controlBarPrefab;
    private GameObject UIParentGO, titleBarGO, controlBarGO;

    RectTransform titleBarRT, controlBarRT;

    private void Awake()
    {
        commonRoot = transform.parent.gameObject.GetComponent<RectTransform>();
        thisRect = GetComponent<RectTransform>();


        titleBarGO = Instantiate(titleBarPrefab, commonRoot);
        titleBarRT = titleBarGO.GetComponent<RectTransform>();
        titleBarGO.SetActive(false);

        controlBarGO = Instantiate(controlBarPrefab, commonRoot);
        controlBarRT = titleBarGO.GetComponent<RectTransform>();
        controlBarGO.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowTitle();
        ShowControlBar();
    }

    private void ShowTitle()
    {
        titleBarGO.SetActive(true);
    }

    private void ShowControlBar()
    {
        controlBarGO.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRect();
    }

    private void UpdateRect()
    {
        if (titleBarGO.activeSelf) thisRect.offsetMax = new Vector2(0, -titleBarRT.rect.height);
        else thisRect.offsetMin = new Vector2(0, 0);

        if (controlBarGO.activeSelf) thisRect.offsetMin = new Vector2(0, controlBarRT.rect.height);
        else thisRect.offsetMin = new Vector2(0, 0);

    }
}

