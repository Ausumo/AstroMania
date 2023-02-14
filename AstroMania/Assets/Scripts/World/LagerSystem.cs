using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LagerSystem : MonoBehaviour
{
    [SerializeField] private bool _isInteract;
    [SerializeField] private bool _isOnLager;
    [SerializeField] private string _playerTag;
    [SerializeField] private int _rocketFuel;

    private GameObject _playerGO;

    [Header("InputActions")]
    [SerializeField] private InputActionReference _interact;

    [Header("UI")]
    [SerializeField] private GameObject _lagerPanel;
    [SerializeField] private Slider _rocketFuelSlider;

    [Header("Camera")]
    [SerializeField] private GameObject _playerCameraController;

    [Header("PlayerMovement")]
    [SerializeField] private PlayerMovement _playerMove;



    private void Start()
    {
        _lagerPanel.SetActive(false);
    }

    private void Update()
    {
        OnInteract();

        OpenLager();
    }

    /// <summary>
    /// Wird benötigt um den Imput für das Interact abzufangen
    /// </summary>
    private void OnInteract()
    {
        float interact = _interact.action.ReadValue<float>();

        if (interact > 0)
            _isInteract = true;
        else
        {
            _isInteract = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == _playerTag)
        {
            _playerGO = collision.gameObject;
            _isOnLager = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == _playerTag)
        {
            _isOnLager = false;
            _playerGO = null;
            Close();
        }
    }

    /// <summary>
    /// Öfnnet das Lager wenn 2 Werte boolisch auf true gesetzt sind (_isOnLager && _isInteract)
    /// </summary>
    private void OpenLager()
    {
        if(_isOnLager && _isInteract)
        {
            _lagerPanel.GetComponent<Menu>().SelectFirstButton();

            //Resetet die Lunge bzw. befüllt die Luftflaschen wieder
            RespiratorySystem.Instance.ResetLungVolume();

            //Lager Panel aktiv
            _lagerPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _playerCameraController.SetActive(false);
            _playerMove.enabled = false;
        }
    }

    /// <summary>
    /// Wrrd vom Button ausgeführt zum schließen des Lagers
    /// </summary>
    public void Close()
    {
        CloseLager();
    }

    /// <summary>
    /// Schliesst das Lager beim entfernen des Spielers aus dem collider 
    /// </summary>
    private void CloseLager()
    {
        //Lager Panel inaktiv

        _isOnLager = false;
        _lagerPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerCameraController.SetActive(true);
        _playerMove.enabled = true;
    }

    /// <summary>
    /// Befüllt die Rackete mit dem Wert den der Spieler gesammelt hat
    /// </summary>
    public void FillRocket()
    {
        int playerFuel = _playerGO.GetComponent<FuelSystem>().playerFuel;

        AddRocketFuel(playerFuel);

        _playerGO.GetComponent<FuelSystem>().ResetPlayerFuel();
    }

    private void UpdatePlayerFuelSlider()
    {
        _rocketFuelSlider.value = _rocketFuel;
    }

    /// <summary>
    /// Added Fuel zum Rocket Inventory
    /// </summary>
    /// <param name="playerFuel"></param>
    private void AddRocketFuel(int playerFuel)
    {
        this._rocketFuel += playerFuel;
        UpdatePlayerFuelSlider();
    }

    /// <summary>
    /// Interactions Enable and Disable
    /// </summary>
    private void OnEnable()
    {
        _interact.action.Enable();
    }

    private void OnDisable()
    {
        _interact.action.Disable();
    }
}
