using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Verwalten des Player-Fuel-Inventars und Aktualisierung der zugehörigen UI.
/// Naming-Convention: private Felder beginnen mit "_" (camelCase), öffentliche Felder in camelCase.
/// </summary>
public class FuelSystem : MonoBehaviour
{
    [FormerlySerializedAs("playerFuel")]
    public int playerFuel;

    [FormerlySerializedAs("_maxFuel")]
    [SerializeField]
    private int _maxFuel = 10;

    [FormerlySerializedAs("_playerFuelSlider")]
    [SerializeField]
    private Slider _playerFuelSlider;

    private void Start()
    {
        ResetPlayerFuel();
    }

    /// <summary>
    /// Aktualisiert den Slider mit dem aktuellen Fuel-Wert.
    /// </summary>
    public void UpdatePlayerFuelSlider()
    {
        if (_playerFuelSlider != null)
            _playerFuelSlider.value = playerFuel;
    }

    /// <summary>
    /// Fügt Fuel zum Spieler-Inventar hinzu und aktualisiert die Anzeige.
    /// </summary>
    /// <param name="amount">Menge an Fuel, die hinzugefügt wird.</param>
    public void AddPlayerFuel(int amount)
    {
        playerFuel += amount;
        UpdatePlayerFuelSlider();
    }

    /// <summary>
    /// Setzt den Fuel-Wert des Spielers auf 0 und aktualisiert die Anzeige.
    /// </summary>
    public void ResetPlayerFuel()
    {
        playerFuel = 0;
        UpdatePlayerFuelSlider();
    }
}
