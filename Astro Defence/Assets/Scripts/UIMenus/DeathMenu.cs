using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    public Text scoreText;
    private void OnEnable()
    {
        scoreText.text = "YOUR SCORE: " + GUI_Manager.SharedInstance.GetScore().ToString();
    }
}
