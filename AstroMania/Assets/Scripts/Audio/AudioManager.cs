using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;

    public AudioSource musicSource, sfxSource;

    //MasterMixer
    [SerializeField]
    private AudioMixer Master;

    //All Audio Mixer - Master - All Music and Sounds
    [Range(0.0001f, 1f)]
    public float masterVolume;
    //Music - Background
    [Range(0.0001f, 1f)]
    public float musicVolume;
    //Sounds - Gameplay 
    [Range(0.0001f, 1f)]
    public float soundsVolume;

    //Instance
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    /// <summary>
    /// Spielt Music ab
    /// </summary>
    /// <param name="name"></param>
    public void PlayMusic(string name)
    {
        Sound music = Array.Find(musicSounds, m => m.name == name);

        if (music == null)
        {
            Debug.Log($"Music '{name}' not found");
        }
        else
        {
            musicSource.clip = music.clip;
            musicSource.Play();
        }
    }

    /// <summary>
    /// Stoppt die aktive Music 
    /// </summary>
    /// <param name="name"></param>
    public void StopMusic(string name)
    {
        Sound music = Array.Find(musicSounds, m => m.name == name);

        if (music == null)
        {
            Debug.Log($"Music '{name}' not found");
        }
        else
        {
            musicSource.clip = music.clip;
            musicSource.Stop();
        }
    }

    /// <summary>
    /// SPielt einen Sound ab
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == name);

        if (sound == null)
        {
            Debug.Log($"Sound '{name}' not found");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    /// <summary>
    /// Stoppt den aktiven SOund
    /// </summary>
    /// <param name="name"></param>
    public void StopSound(string name)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == name);

        if (sound == null)
        {
            Debug.Log($"Sound '{name}' not found");
        }
        else
        {
            sfxSource.clip = sound.clip;
            sfxSource.Stop();
        }
    }

    /// <summary>
    /// Wird von der Update Methode ausgeführt und hält alle Volumes aktuell
    /// </summary>
    public void SetVolumes()
    {
        Master.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        Master.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        Master.SetFloat("SoundsVolume", Mathf.Log10(soundsVolume) * 20);
    }
}
