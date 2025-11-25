using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Verwaltet das Lager-Interaction-System: Öffnen/Schließen des Lager-Panels, Befüllen der Rakete und UI-Updates.
/// Naming-Convention: private Felder beginnen mit "_" (camelCase), öffentliche Felder in camelCase.
/// </summary>
public class LagerSystem : MonoBehaviour
{
    [FormerlySerializedAs("isOnLager")]
    public bool isOnLager;

    [FormerlySerializedAs("_isInteract")]
    [SerializeField] private bool _isInteract;

    [FormerlySerializedAs("_stayOnLager")]
    [SerializeField] private bool _stayOnLager;

    [FormerlySerializedAs("_playerTag")]
    [SerializeField] private string _playerTag;

    [FormerlySerializedAs("rocketFuel")]
    public int rocketFuel;

    [FormerlySerializedAs("isPlayerWin")]
    public bool isPlayerWin;

    [FormerlySerializedAs("_maxRocketFuel")]
    [SerializeField] private int _maxRocketFuel = 50;

    private GameObject _playerGO;

    [Header("InputActions")]
    [FormerlySerializedAs("_interact")]
    [SerializeField] private InputActionReference _interact;

    [Header("UI")]
    [FormerlySerializedAs("_lagerPanel")]
    [SerializeField] private GameObject _lagerPanel;

    [FormerlySerializedAs("_rocketFuelSlider")]
    [SerializeField] private Slider _rocketFuelSlider;

    [FormerlySerializedAs("_startRocketButton")]
    [SerializeField] private Button _startRocketButton;

    [Header("Camera")]
    [FormerlySerializedAs("_playerCameraController")]
    [SerializeField] private GameObject _playerCameraController;

    [Header("PlayerMovement")]
    [FormerlySerializedAs("_playerMove")]
    [SerializeField] private PlayerMovement _playerMove;

    private bool _isPaused;

    private void Start()
    {
        if (_startRocketButton != null)
            _startRocketButton.interactable = false;

        if (_lagerPanel != null)
            _lagerPanel.SetActive(false);
    }

    private void Update()
    {
        // Input für Interact abfragen
        OnInteract();

        // Lager öffnen, wenn Bedingungen erfüllt
        OpenLager();
    }

    /// <summary>
    /// Liest die Interact-InputAction (Analogwert oder Button)
    /// </summary>
    private void OnInteract()
    {
        if (_interact == null) return;

        float interact = _interact.action.ReadValue<float>();
        _isInteract = interact > 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        // Verwendung von CompareTag ist effizienter
        if (other.CompareTag(_playerTag))
        {
            _playerGO = other.gameObject;
            _stayOnLager = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            _stayOnLager = false;
            _playerGO = null;
            Close();
        }
    }

    /// <summary>
    /// Öffnet das Lagerpanel, wenn der Spieler auf dem Lager ist und Interact drückt.
    /// Versteckt die Spieler-Kamera, deaktiviert PlayerMovement und setzt UI.
    /// </summary>
    private void OpenLager()
    {
        var pauseManager = FindObjectOfType<PauseManager>();
        _isPaused = pauseManager != null && pauseManager.isPaused;

        if (!_isPaused)
        {
            if (_stayOnLager && _isInteract && _playerGO != null)
            {
                var menu = _lagerPanel?.GetComponent<Menu>();
                menu?.SelectFirstButton();

                // Reset der Lungen-/Sauerstoffanzeige des Spielers
                var respiratory = _playerGO.GetComponent<RespiratorySystem>();
                respiratory?.ResetLungVolume();

                // Lager Panel aktivieren und UI anpassen
                isOnLager = true;
                _lagerPanel?.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (_playerCameraController != null) _playerCameraController.SetActive(false);
                if (_playerMove != null) _playerMove.enabled = false;

                if (rocketFuel == _maxRocketFuel && _startRocketButton != null)
                {
                    _startRocketButton.interactable = true;
                }
            }
        }
    }

    /// <summary>
    /// Wird vom Button aufgerufen, um das Lager zu schließen.
    /// </summary>
    public void Close()
    {
        CloseLager();
    }

    /// <summary>
    /// Schließt das Lager: Panel deaktivieren, Spieler wieder aktivieren.
    /// </summary>
    private void CloseLager()
    {
        isOnLager = false;
        _stayOnLager = false;

        if (_lagerPanel != null) _lagerPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (_playerCameraController != null) _playerCameraController.SetActive(true);
        if (_playerMove != null) _playerMove.enabled = true;
    }

    /// <summary>
    /// Befüllt die Rakete mit dem Fuel des Spielers und leert das Inventar des Spielers.
    /// </summary>
    public void FillRocket()
    {
        if (_playerGO == null) return;

        var fuelSystem = _playerGO.GetComponent<FuelSystem>();
        if (fuelSystem == null) return;

        int playerFuel = fuelSystem.playerFuel;
        AddRocketFuel(playerFuel);
        fuelSystem.ResetPlayerFuel();
    }

    /// <summary>
    /// Aktualisiert den Rocket-Fuel-Slider.
    /// </summary>
    public void UpdateRocketFuelSlider()
    {
        if (_rocketFuelSlider != null)
            _rocketFuelSlider.value = rocketFuel;
    }

    /// <summary>
    /// Fügt Fuel zur Rakete hinzu und aktualisiert UI.
    /// </summary>
    private void AddRocketFuel(int playerFuel)
    {
        this.rocketFuel += playerFuel;
        UpdateRocketFuelSlider();
    }

    public void ResetRocketFuel()
    {
        rocketFuel = 0;
        UpdateRocketFuelSlider();
    }

    /// <summary>
    /// Startet die Rakete (setzt Win-Flag). Weitere Logik kann extern implementiert werden.
    /// </summary>
    public void StartRocket()
    {
        isPlayerWin = true;
    }

    // InputActions aktivieren / deaktivieren
    private void OnEnable()
    {
        if (_interact != null) _interact.action.Enable();
    }

    private void OnDisable()
    {
        if (_interact != null) _interact.action.Disable();
    }
}
