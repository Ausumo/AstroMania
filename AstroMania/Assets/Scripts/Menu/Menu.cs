using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private int _menu;

    [Header("FirstSelectedButton")]
    [SerializeField] private GameObject _firstSelected;

    private void Start()
    {

    }

    /// <summary>
    /// Setze den ersten Button auf selected wenn das menu geöffnet wird
    /// </summary>
    public void SelectFirstButton()
    {
        _firstSelected.GetComponent<Selectable>().Select();
    }
}
