using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    //Graphic Setting int
    [SerializeField]
    private int _graphicSettings;

    //GameManager
    [SerializeField]
    private GameManager _gameManager;

    //DropDown Graphic Quality
    [SerializeField]
    private TMP_Dropdown _dropDownGraphics;

    private void Start()
    {
        UpdateSliderFromVolume();
        UpdateDropDownMenuFromGraphicSettings();
    }

    /// <summary>
    /// Updated die Volumes von den Slidern
    /// </summary>
    public void UpdateVolumeFromSlider()
    {
        AudioManager.Instance.masterVolume = _masterVolumeSlider.value;
        AudioManager.Instance.musicVolume = _musicVolumeSlider.value;
        AudioManager.Instance.soundsVolume = _soundsVolumeSlider.value;
        AudioManager.Instance.SetVolumes();
    }

    public void UpdatePostProcessFromDropDown()
    {
        PostProcessingManager.Instance.graphicSettings = _dropDownGraphics.value;
        PostProcessingManager.Instance.SetGraphics();
    }

    /// <summary>
    /// Updated den Slider von den Volumes
    /// </summary>
    public void UpdateSliderFromVolume()
    {
        //Nur die erste Zeile wird ausgeführt. Nur wenn ich es davor in extra eigene Variablen abspeichere geht es
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

    public void UpdateDropDownMenuFromGraphicSettings()
    {
        int graphic = 0;

        graphic = PostProcessingManager.Instance.graphicSettings;

        _dropDownGraphics.value = graphic;
    }
}
