using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Zentrale GameManager-Singleton-Klasse für persistente Spielzustände und Options-Speicherung/Laden.
/// Naming-Convention: private Felder beginnen mit "_" (camelCase), öffentliche Felder in camelCase.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton-Instanz (private)
    private static GameManager _instance;

    /// <summary>
    /// Zugriff auf die globale GameManager-Instanz.
    /// Hinweis: Die Instanz wird in Awake gesetzt. Instance kann null sein, wenn noch keine Instanz existiert.
    /// </summary>
    public static GameManager Instance => _instance;

    [FormerlySerializedAs("isGameLoaded")]
    public bool isGameLoaded;

    private void Awake()
    {
        // Standard-Singleton-Pattern: Wenn es bereits eine Instanz gibt, zerstören wir dieses GameObject.
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region Save and Load Options
    /// <summary>
    /// Speichert alle Werte des Options-Menüs in PlayerPrefs.
    /// Nutzt vorhandene Manager (AudioManager, PostProcessingManager) falls vorhanden.
    /// </summary>
    public void SaveOptions()
    {
        // Sicherstellen, dass AudioManager existiert
        if (AudioManager.Instance != null)
        {
            PlayerPrefs.SetFloat("MasterVolume", AudioManager.Instance.masterVolume);
            PlayerPrefs.SetFloat("MusicVolume", AudioManager.Instance.musicVolume);
            PlayerPrefs.SetFloat("SoundVolume", AudioManager.Instance.soundsVolume);
        }

        // Sicherstellen, dass PostProcessingManager existiert
        if (PostProcessingManager.Instance != null)
        {
            PlayerPrefs.SetInt("GraphicSettings", PostProcessingManager.Instance.graphicSettings);
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Lädt die Werte des Options-Menüs aus PlayerPrefs.
    /// Verwendet sinnvolle Defaults, falls Keys nicht existieren.
    /// </summary>
    public void LoadOptions()
    {
        // Audio-Einstellungen laden, falls vorhanden
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.masterVolume = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : AudioManager.Instance.masterVolume;
            AudioManager.Instance.musicVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : AudioManager.Instance.musicVolume;
            AudioManager.Instance.soundsVolume = PlayerPrefs.HasKey("SoundVolume") ? PlayerPrefs.GetFloat("SoundVolume") : AudioManager.Instance.soundsVolume;

            AudioManager.Instance.SetVolumes();
        }

        // Grafik-Einstellungen laden, falls vorhanden
        if (PostProcessingManager.Instance != null)
        {
            PostProcessingManager.Instance.graphicSettings = PlayerPrefs.HasKey("GraphicSettings") ? PlayerPrefs.GetInt("GraphicSettings") : PostProcessingManager.Instance.graphicSettings;
            PostProcessingManager.Instance.SetGraphics();
        }
    }
    #endregion

    /// <summary>
    /// Externer Setter, um anzugeben, ob das Spiel geladen wurde.
    /// </summary>
    public void IsGameLoaded(bool isLoaded)
    {
        isGameLoaded = isLoaded;
    }
}
