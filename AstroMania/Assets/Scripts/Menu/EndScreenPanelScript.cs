using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenPanelScript : MonoBehaviour
{
    [SerializeField]
    private Button _backToMenuButton;


    public void ShowCursor()
    {
        Cursor.visible = false;

        _backToMenuButton.Select();
    }
}
