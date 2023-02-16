using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespiratorySystem : MonoBehaviour
{
    public int lungVolume;
    [SerializeField]
    private int _maxLungVolume = 100;
    [SerializeField]
    private float _lungSpeed = 2;

    [SerializeField]
    private Slider _playerLungSlider;

    [SerializeField]
    private Menu _deadScreen;

    [HideInInspector]
    public bool _isDead;

    private void Start()
    {
        ResetLungVolume();
        StartCoroutine(RemoveLungVolume());
    }

    private void Update()
    {
        if (lungVolume == 30)
        {
            //Freeze Screen Updaten -> Wenn zeit bleibt
            print("Noch 30 Prozent");
        }
        else if (lungVolume == 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        gameObject.GetComponent<Rigidbody>().freezeRotation = false;

        _deadScreen.gameObject.SetActive(true);
        _isDead = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Reset the lungVolume to max
    /// </summary>
    public void ResetLungVolume()
    {
        lungVolume = _maxLungVolume;
        UpdateLungSlider();
    }

    /// <summary>
    /// Updated den Slider
    /// </summary>
    private void UpdateLungSlider()
    {
        _playerLungSlider.value = lungVolume;
    }

    /// <summary>
    /// Zieht mit dem angegebenen Speed die Lungen Kapazität ab
    /// </summary>
    /// <returns></returns>
    private IEnumerator RemoveLungVolume() 
    {
        while(lungVolume > 0)
        {
            --lungVolume;
            UpdateLungSlider();

            yield return new WaitForSeconds(_lungSpeed);
        }

        yield return null;
    }
}
