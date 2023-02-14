using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelSystem : MonoBehaviour
{
    public int playerFuel;

    [SerializeField]
    private int _maxFuel = 10;

    [SerializeField]
    private Slider _playerFuelSlider;

    /// <summary>
    /// Update the Player Fuel Slider
    /// </summary>
    private void UpdatePlayerFuelSlider()
    {
        _playerFuelSlider.value = playerFuel;
    }

    /// <summary>
    /// Added Fuel zum Player Inventory
    /// </summary>
    /// <param name="playerFuel"></param>
    public void AddPlayerFuel(int playerFuel)
    {
        this.playerFuel += playerFuel;
        UpdatePlayerFuelSlider();
    }

    /// <summary>
    /// Setzt den PlayerFuel wieder auf 0 um die anzeige Upzudaten das sie leer ist
    /// </summary>
    public void ResetPlayerFuel()
    {
        this.playerFuel = 0;
        UpdatePlayerFuelSlider();
    }
}
