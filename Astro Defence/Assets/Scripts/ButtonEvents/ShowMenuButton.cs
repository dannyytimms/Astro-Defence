using UnityEngine;

public class ShowMenuButton : MonoBehaviour
{
    public void OnClick(string menuName)
    {
        GUI_Manager.SharedInstance.ToggleMenu(menuName);
    }
}
