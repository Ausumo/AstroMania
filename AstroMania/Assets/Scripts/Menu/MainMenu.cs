using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {

    }

    /// <summary>
    /// Methode zum Verlassen des Spiels
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
