using Assets.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageListController : MonoBehaviour
{
    private ScrollRect scrollRect;

    public int imagesPerPage = 20;
    public int page = 0;

    public GameObject imageCardPrefab;
    private List<GameObject> pooledImageCards = new List<GameObject>();

    public float coolDownTime = 5.0f;
    public float currentCoolDownTime = 0.0f;

    private float initialVerticalPosition;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        FillContentWithBlanks();
    }

    private void Start()
    {
        initialVerticalPosition = scrollRect.verticalNormalizedPosition;
        JsonManager.SharedInstance.StartDownload((result) => LoadImages(result));

        scrollRect.onValueChanged.AddListener(OnScrollRectChanged);
    }

    private void FillContentWithBlanks()
    {
        for(int i = 0; i < imagesPerPage; i++)
        {
            GameObject cardPrefab = Instantiate(imageCardPrefab, scrollRect.content);
            cardPrefab.SetActive(false);

            if (i % 2 == 0)
            {
                cardPrefab.transform.GetChild(0).SetSiblingIndex(1); //swap every other card
            }

            pooledImageCards.Add(cardPrefab);

        }
    }

    private GameObject GetCard(int index)
    {
        return pooledImageCards[index];
    }

    public void LoadImages(List<PhotoItem> photoItems)
    {
        for (int i = 0; i < imagesPerPage; i++)
        {
            GameObject prefab = GetCard(i);

            if (prefab == null)
                return;

            prefab.SetActive(true);

            prefab.GetComponent<ImageItemController>().SetItemData(photoItems[i].title, photoItems[i].url);
            prefab.transform.SetParent(scrollRect.content);
        }

    }

    private void RefreshPage()
    {
        page += 1;
        LoadImages(JsonManager.SharedInstance.GetSetOfImages(imagesPerPage, page * imagesPerPage));

        scrollRect.verticalScrollbar.value = 1.0f;
        scrollRect.verticalNormalizedPosition = initialVerticalPosition;
    }

    private void OnScrollRectChanged(Vector2 position)
    {
        if (position.y < 0.01f && currentCoolDownTime <= 0.0f)
        {
            currentCoolDownTime = coolDownTime;
            RefreshPage();
        }
    }

    private void Update()
    {
        if (currentCoolDownTime > 0)
            currentCoolDownTime -= Time.deltaTime;
    }
}
