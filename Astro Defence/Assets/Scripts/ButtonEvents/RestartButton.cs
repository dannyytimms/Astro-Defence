using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public void OnClick()
    {
        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
