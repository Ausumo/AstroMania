using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{

    [Header("Menus")]
    [SerializeField] private bool _isPlayMode;

    [SerializeField] private List<Menu> _menuList = new List<Menu>();
    private Menu _activeMenu;

    [Header("InputActions")]
    [SerializeField] private InputActionReference _startMenu;

    void Start()
    {
        if (_isPlayMode)
        {
            //LoadOptions
            GameManager.Instance.LoadOptions();
                
            _activeMenu = _menuList[0];
            SyncMenus();

            for (int i = 1; i < _menuList.Count; i++)
            {
                _menuList[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        //Wenn active scene der startScreen ist
        if (_activeMenu == _menuList[0])
            PressAnyKey();
    }

    #region MenuManager
    /// <summary>
    /// Wenn any key in startscreen gedrückt wird
    /// </summary>
    public void PressAnyKey()
    {
        float isPressAnyKey = _startMenu.action.ReadValue<float>();

        if (isPressAnyKey > 0)
        {
            AudioManager.Instance.PlaySound("ChangeMenu");
            //Wenn irgendein Key gedrückt wird setzt er das Menu vom StartScreen auf das MainMenu
            _activeMenu = _menuList[1];
            _activeMenu.SelectFirstButton();
            SyncMenus();
        }
    }

    /// <summary>
    /// Öffne ein neues Menu
    /// </summary>
    /// <param name="menu"></param>
    public void SetMenu(int menu)
    {
        _activeMenu = _menuList[menu];
        _activeMenu.SelectFirstButton();
        SyncMenus();
    }

    /// <summary>
    /// Setzt das active Menu auf true und alle anderen auf false
    /// </summary>
    private void SyncMenus()
    {
        if (_activeMenu != null)
        {
            for (int i = 0; i < _menuList.Count; i++)
            {
                _menuList[i].gameObject.SetActive(false);
            }

            _activeMenu.gameObject.SetActive(true);
        }
    }
    #endregion

    //InputActions Aktivieren und Deaktivieren
    private void OnEnable()
    {
        _startMenu.action.Enable();
    }

    private void OnDisable()
    {
        _startMenu.action.Disable();
    }


}

