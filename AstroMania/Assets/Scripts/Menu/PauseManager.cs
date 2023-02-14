using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private List<Menu> _menuList = new List<Menu>();
    private Menu _activeMenu;

    [Header("InputActions")]
    [SerializeField] private InputActionReference _pauseKey;

    private bool _isPaused;

    private void Update()
    {
        MakePause();
    }

    public void MakePause()
    {
        bool isPauseKeyPressed = _pauseKey.action.triggered;

        if (isPauseKeyPressed)
        {
            if (_isPaused)
            {
                //Pause Menu aus machen

                SetMenu(0);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _isPaused = false;
                Time.timeScale = 0f;
            }
            else
            {
                //Pause Menu an machen

                DeactivateAllMenus();
                _isPaused = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1f;
            }
        }
    }

    #region Menu Methods
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

    public void DeactivateAllMenus()
    {
        for (int i = 0; i < _menuList.Count; i++)
        {
            _menuList[i].gameObject.SetActive(false);
        }
    }
    #endregion



    //InputActions Aktivieren und Deaktivieren
    private void OnEnable()
    {
        _pauseKey.action.Enable();
    }

    private void OnDisable()
    {
        _pauseKey.action.Disable();
    }

}
