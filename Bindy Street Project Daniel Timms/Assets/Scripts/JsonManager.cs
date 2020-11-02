using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Assets.Models;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Linq;
using System;

public class JsonManager : MonoBehaviour
{
    public List<PhotoItem> photoItems;

    public static JsonManager SharedInstance;

    private void Awake()
    {
        SharedInstance = this;
        //StartCoroutine(DownloadData(()=> SendData()));

    }

    //private void SendData()
    //{
    //    ImageListController.
    //}

    private void Start()
    {
    }

    public void StartDownload(Action<List<PhotoItem>> onCompletion)
    {
        StartCoroutine(DownloadData(onCompletion));
    }

    private IEnumerator DownloadData(Action<List<PhotoItem>> onCompletion)
    {

        UnityWebRequest www = UnityWebRequest.Get("https://jsonplaceholder.typicode.com/photos");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            photoItems = GetJsonImages(www.downloadHandler.text);
            onCompletion(photoItems);
        }
    }

    private List<PhotoItem> GetJsonImages(string jsonData)
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.Log("JsonData is empty");
            return null;
        }

        return JsonConvert.DeserializeObject<List<PhotoItem>>(jsonData);
    }

    public List<PhotoItem> GetSetOfImages(int quantity, int index)
    {
        Debug.Log(photoItems.Count + " count " + index);
        return photoItems.Skip(index).Take(quantity).ToList();
    }
}
