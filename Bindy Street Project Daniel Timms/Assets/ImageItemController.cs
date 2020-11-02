using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageItemController : MonoBehaviour
{
    public Image iconImage;
    public Text titleText;

    public void SetItemData(string title, string imageUrl)
    {
        //iconImage = gameObject.GetComponentInChildren<Image>();
        //titleText = gameObject.GetComponentInChildren<Text>();

        titleText.text = title;
        StartCoroutine(GetImage(imageUrl));
    }

    private IEnumerator GetImage(string url)
    {
        // Start a download of the given URL
        using (WWW www = new WWW(url))
        {
            // Wait for download to complete
            yield return www;

            // assign texture
            iconImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        }

        iconImage.preserveAspect = true;

        //UnityWebRequest www = new UnityWebRequest(url);
        //yield return www.SendWebRequest();

        ////Texture2D imageTexture;

        //if (www.isNetworkError || www.isHttpError)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    Debug.Log(www.downloadHandler.data);
        //   // imageTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        //    iconImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, imageTexture.width, imageTexture.height), new Vector2(0, 0));
        //}
    }
}
