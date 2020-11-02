using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public string menuName = "none";

    private MenuManager _manager;
    public MenuManager Manager
    {
        get { return _manager; }
        set { _manager = value; }
    }

    // ===================================================================
    public virtual void PlayIn(string comingFromMenu)
    {
        gameObject.SetActive(true);
    }

    // ===================================================================
    public virtual void PlayInComplete()
    {

    }

    // ===================================================================
    public virtual void PlayOut(string nextMenu)
    {

    }

    // ===================================================================
    public virtual void PlayOutComplete()
    {
        gameObject.SetActive(false);
    }

    // ===================================================================
    public virtual void GoToMenu(string menuName)
    {
        Manager.GoTo(menuName);
    }

    // ===================================================================
    public virtual void GoToPreviousMenu()
    {
        Manager.GoPrevious();
    }
}
