using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField] private string _stoneTag = "Stone";

    [Header("InputActions")]
    [SerializeField] private InputActionReference _interact;

    private FuelStones _stone;

    private void Update()
    {
        OnInteract();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == _stoneTag)
        {
            _stone = other.gameObject.GetComponent<FuelStones>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == _stoneTag)
        {
            _stone.StopMining();
            _stone = null;
        }
    }

    /// <summary>
    /// Wird benötigt um den Imput für das Interact abzufangen
    /// </summary>
    private void OnInteract()
    {
        bool interact = _interact.action.triggered;

        if (interact)
        {
            if (_stone != null) 
            {
                _stone.MineStone(gameObject.GetComponent<FuelSystem>());
            }
        }
        else
        {
            if (_stone != null)
            {
                _stone.StopMining();
            }
        }
    }
}
