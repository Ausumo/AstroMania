using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LagerSystem : MonoBehaviour
{
    public bool isOnLager;

    [SerializeField] private bool _isInteract;
    [SerializeField] private bool _stayOnLager;
    [SerializeField] private string _playerTag;

    public int rocketFuel;

    public bool isPlayerWin;

    [SerializeField] private int _maxRocketFuel = 50;

    private GameObject _playerGO;

    [Header("InputActions")]
    [SerializeField] private InputActionReference _interact;

    [Header("UI")]
    [SerializeField] private GameObject _lagerPanel;
    [SerializeField] private Slider _rocketFuelSlider;
    [SerializeField] private Button _startRocketButton;

    [Header("Camera")]
    [SerializeField] private GameObject _playerCameraController;

    [Header("PlayerMovement")]
    [SerializeField] private PlayerMovement _playerMove;

    private bool _isPaused;


    private void Start()
    {
        _startRocketButton.interactable = false;
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

    private void OnTriggerStay(Collider collision)
    {
        if(collision.gameObject.tag == _playerTag)
        {
            _playerGO = collision.gameObject;
            _stayOnLager = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == _playerTag)
        {
            _stayOnLager = false;
            _playerGO = null;
            Close();
        }
    }

    /// <summary>
    /// Öfnnet das Lager wenn 2 Werte boolisch auf true gesetzt sind (_isOnLager && _isInteract)
    /// </summary>
    private void OpenLager()
    {
        _isPaused = FindObjectOfType<PauseManager>().GetComponent<PauseManager>().isPaused;

        if(!_isPaused )
        {
            if (_stayOnLager && _isInteract)
            {
                _lagerPanel.GetComponent<Menu>().SelectFirstButton();

                //Resetet die Lunge bzw. befüllt die Luftflaschen wieder
                _playerGO.GetComponent<RespiratorySystem>().ResetLungVolume();

                //Lager Panel aktiv
                isOnLager = true;
                _lagerPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                _playerCameraController.SetActive(false);
                _playerMove.enabled = false;

                if(rocketFuel == _maxRocketFuel)
                {
                    _startRocketButton.interactable = true;
                    isPlayerWin = true;
                }
            }
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
        isOnLager = false;
        _stayOnLager = false;
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

    public void UpdateRocketFuelSlider()
    {
        _rocketFuelSlider.value = rocketFuel;
    }

    /// <summary>
    /// Added Fuel zum Rocket Inventory
    /// </summary>
    /// <param name="playerFuel"></param>
    private void AddRocketFuel(int playerFuel)
    {
        this.rocketFuel += playerFuel;
        UpdateRocketFuelSlider();
    }

    private void ResetRocketFuel()
    {
        rocketFuel = 0;
        UpdateRocketFuelSlider();
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
