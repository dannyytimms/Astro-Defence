using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public List<Menu> menuList;

    public string OverrideMenuStart;

    private string initialMenu;
    private string currentMenu;
    private string _previousMenu;

    public MenuManager parentMenuManager;

    private Dictionary<string, Menu> _menus;

    public string CurrentMenu
    {
        get { return currentMenu; }

        set
        {
            currentMenu = value;
            UIOverlayController.SharedInstance.SetTitle(currentMenu);
        }
    }

    // ===================================================================
    void Start()
    {
        initialMenu = OverrideMenuStart;
        
        _menus = new Dictionary<string, Menu>();
        foreach (Menu m in menuList)
        {
            _menus.Add(m.menuName, m);
        }

        foreach (Menu m in menuList)
        {
            m.gameObject.SetActive(false);
            m.Manager = this;
        }

        currentMenu = initialMenu;
        Menu firstMenu;
        _menus.TryGetValue(initialMenu, out firstMenu);

        CurrentMenu = firstMenu.menuName;
        firstMenu.gameObject.SetActive(true);
    }

    // ===================================================================
    public void GoTo(string nextMenu)
    {
        HandleGoTo(nextMenu);
    }

    private void HandleGoTo(string nextMenu)
    {
        if (nextMenu == currentMenu)
            return;

        _previousMenu = currentMenu;

        Menu currentM, nextM;


        _menus.TryGetValue(currentMenu, out currentM);
        _menus.TryGetValue(nextMenu, out nextM);

        currentM.PlayOut(nextMenu);

        nextM.PlayIn(currentMenu);

        currentMenu = nextMenu;
    }

    // ===================================================================
    public void GoPrevious()
    {
        GoTo(_previousMenu);
    }

    // ==================================================================
    public Menu getMenu(string name)
    {
        foreach (Menu m in menuList)
            if (m.menuName == name)
                return m;

        return null;
    }

    // ==================================================================
    public string getPreviousMenuName()
    {
        return _previousMenu;
    }
}
