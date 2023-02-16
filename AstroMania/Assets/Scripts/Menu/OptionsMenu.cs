using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class OptionsMenu : MonoBehaviour
{
    //Master Volume Slider
    [SerializeField]
    private Slider _masterVolumeSlider;

    //Music Volume Slider
    [SerializeField]
    private Slider _musicVolumeSlider;

    //Master Volume Slider
    [SerializeField]
    private Slider _soundsVolumeSlider;

    //DropDown Graphic Quality
    [SerializeField]
    private TMP_Dropdown _graphicQualityDropdown;

    //GameManager
    [SerializeField]
    private GameManager _gameManager;

    private void Start()
    {
        UpdateSliderFromVolume();
    }

    public void UpdateVolumeFromSlider()
    {
        AudioManager.Instance.masterVolume = _masterVolumeSlider.value;
        AudioManager.Instance.musicVolume = _musicVolumeSlider.value;
        AudioManager.Instance.soundsVolume = _soundsVolumeSlider.value;
        AudioManager.Instance.SetVolumes();
    }

    public void UpdateSliderFromVolume()
    {
        //Nur die erste Zeile wird ausgeführt nur wenn ich es davor in extra eigene Variablen abspeichere
        float masterVolume = 0;
        float musicVolume = 0;
        float soundVolume = 0;

        masterVolume = AudioManager.Instance.masterVolume;
        musicVolume = AudioManager.Instance.musicVolume;
        soundVolume = AudioManager.Instance.soundsVolume;

        _masterVolumeSlider.value = masterVolume;
        _musicVolumeSlider.value = musicVolume;
        _soundsVolumeSlider.value = soundVolume;
    }
}
