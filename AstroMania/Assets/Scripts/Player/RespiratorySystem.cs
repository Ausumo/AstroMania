using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespiratorySystem : MonoBehaviour
{
    //Instance
    private static RespiratorySystem _instance;
    public static RespiratorySystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RespiratorySystem>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public int lungVolume;
    [SerializeField]
    private int _maxLungVolume = 100;
    [SerializeField]
    private float _lungSpeed = 2;

    [SerializeField]
    private Slider _playerLungSlider;

    private void Start()
    {
        StartCoroutine(RemoveLungVolume());
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
