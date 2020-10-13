using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public void OnClick()
    {
        GUI_Manager.SharedInstance.SetPauseState();
    }
}
