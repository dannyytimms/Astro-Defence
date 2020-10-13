using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void OnClick(string nextMenu)
    {
        GUI_Manager.SharedInstance.ToggleMenu(nextMenu);
    }
}
