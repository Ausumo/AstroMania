using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    //Instance
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        { 
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    
    public bool isGameLoaded;

    #region Save and Load Options
    /// <summary>
    /// Speichert alle Werte des OptionsMenu
    /// </summary>
    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("MasterVolume", AudioManager.Instance.masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", AudioManager.Instance.musicVolume);
        PlayerPrefs.SetFloat("SoundVolume", AudioManager.Instance.soundsVolume);
        PlayerPrefs.SetInt("GraphicSettings", PostProcessingManager.Instance.graphicSettings);

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Lädt alle Werte des OptionsMenu
    /// </summary>
    public void LoadOptions()
    {
        AudioManager.Instance.masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        AudioManager.Instance.musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        AudioManager.Instance.soundsVolume = PlayerPrefs.GetFloat("SoundVolume");
        PostProcessingManager.Instance.graphicSettings = PlayerPrefs.GetInt("GraphicSettings");

        AudioManager.Instance.SetVolumes();
        PostProcessingManager.Instance.SetGraphics();
    }
    #endregion

    public void IsGameLoaded(bool isLoaded)
    {
        isGameLoaded = isLoaded;
    }
}
