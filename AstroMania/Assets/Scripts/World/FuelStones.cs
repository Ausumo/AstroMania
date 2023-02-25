using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FuelStones : MonoBehaviour
{
    [Range(0f, 3f)]
    public int fuelVolume;

    private FuelSystem _collSystem;

    /// <summary>
    /// Wird ausgeführt wenn der SPieler den Stein abbaut
    /// </summary>
    public void MineStone(FuelSystem collSystem)
    {
        _collSystem= collSystem;
        StartCoroutine(StoneMining());
        //stone mining coroutine
        //Play animation
        //Destroy Stone

    }

    private IEnumerator StoneMining()
    {

        //Animation Broke Stone
        yield return new WaitForSeconds(fuelVolume);
        _collSystem.AddPlayerFuel(fuelVolume);
        gameObject.SetActive(false);
    }

    public void StopMining()
    {
        StopCoroutine(StoneMining());
    }
}
