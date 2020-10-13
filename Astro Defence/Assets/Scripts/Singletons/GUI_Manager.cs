using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Menu
{
    public string MenuName;
    public GameObject MenuObject;
}

public class GUI_Manager : MonoBehaviour
{
    public static GUI_Manager SharedInstance;

    public Canvas mainCanvas;
    [Space(10)]
    public Vector3 BottomLeftCorner, TopLeftCorner, TopRightCorner, BottomRightCorner;
    [Space(10)]

    public float playerSpaceOffset = 1.5f; //we dont want any obstacles to spawn horizontally to the player so we give it an offset.
    [Space(10)]

    public Text scoreText, livesText;
    public int score = 0, lives = 3;
    [Space(10)]

    public List<Menu> menus = new List<Menu>();

    private float canvasHeight;

    private Camera mainCamera;

    public bool isPaused { get; private set; }
    public float get_height()
    {
        return canvasHeight;
    }

    private float canvasWidth;
    public float get_width()
    {
        return canvasWidth;
    }


    private void Awake()
    {
        SharedInstance = this;

        mainCamera = Camera.main;
        mainCanvas = mainCamera.GetComponentInChildren<Canvas>();
        RectTransform rt = mainCanvas.GetComponent<RectTransform>();
        GetWorldCorners(rt); //so we know where to spawn our obstacles!
    }

    private void Start()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
    }

    public void AddScore(int multiplier = 1)
    {
        score +=  1 * multiplier;
        scoreText.text = "Score: " + score;
    }

    public int GetScore()
    {
        return score;
    }

    public void RemoveLife()
    {
        livesText.text = "Lives: " + --lives;
    }

    public void DisplayMenu(string menuName)
    {
        ToggleMenu(menuName);
    }

    public void ToggleMenu(string menuName = null, string nextMenu = null)
    {
        if(menuName == null) menuName = GetCurrentMenuName();

        if (nextMenu == null)
        {
            foreach (Menu m in menus)
            {
                if (m.MenuName != menuName)
                    m.MenuObject.SetActive(false);
                else
                    m.MenuObject.SetActive(true);
            }
        }
        else
        {            
            foreach (Menu m in menus)
            {
                m.MenuObject.SetActive(false);
            }

            menus.Where(item => item.MenuName == nextMenu).FirstOrDefault().MenuObject.SetActive(true);
        }
    }

    public string GetCurrentMenuName()
    {
        return menus.Where(item => item.MenuObject.activeSelf == true).FirstOrDefault().MenuName;
    }

    public void SetPauseState()
    {
        isPaused = !isPaused;
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        ToggleMenu("Pause", isPaused == true ? null : "Data");
    }

    private void GetWorldCorners(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        canvasHeight = corners[1].y - corners[0].y;
        canvasWidth = corners[3].x - corners[0].x;

        BottomLeftCorner = corners[0];
        BottomLeftCorner.y += playerSpaceOffset;

        TopLeftCorner = corners[1];
        TopRightCorner = corners[2];

        BottomRightCorner = corners[3];
        BottomRightCorner.y += playerSpaceOffset;
    }
}
