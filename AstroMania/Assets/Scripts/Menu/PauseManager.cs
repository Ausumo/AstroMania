using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private List<Menu> _menuList = new List<Menu>();
    private Menu _activeMenu;

    [Header("Camera")]
    [SerializeField] private GameObject _playerCameraController;

    [Header("InputActions")]
    [SerializeField] private InputActionReference _pauseKey;

    public bool isPaused;

    private bool _isDead;
    private bool _isOnLager;

    private void Update()
    {
        MakePause();
    }

    /// <summary>
    /// Wird immer ausgeführt aber nur wenn Escape gedrückt wird passiert etwas
    /// </summary>
    public void MakePause()
    {
        _isDead = FindObjectOfType<RespiratorySystem>().GetComponent<RespiratorySystem>()._isDead;
        _isOnLager = FindObjectOfType<LagerSystem>().GetComponent<LagerSystem>().isOnLager;

        bool isPauseKeyPressed = _pauseKey.action.triggered;

        if (isPauseKeyPressed)
        {
            
            if(!_isDead && !_isOnLager)
            {
                if (!isPaused)
                {
                    //Pause Menu aus machen

                    SetMenu(0);
                    TimeOff();
                    _playerCameraController.SetActive(false);
                    isPaused = true;
                }
                else
                {
                    //Pause Menu an machen

                    DeactivateAllMenus();
                    isPaused = false;
                }
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
        TimeOn();
        isPaused = false;
        _playerCameraController.SetActive(true);

        for (int i = 0; i < _menuList.Count; i++)
        {
            _menuList[i].gameObject.SetActive(false);
        }
    }
    #endregion

    /// <summary>
    /// Deaktiviert nur den Pause Screen
    /// </summary>
    public void DeactivatePause()
    {
        isPaused = false;
        TimeOn();
    }

    /// <summary>
    /// Cursor weg und Time auf 1
    /// </summary>
    private void TimeOn()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Cursor da und Time auf 0
    /// </summary>
    private void TimeOff()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

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
