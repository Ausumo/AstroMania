using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{

    private AudioManager _audioManager;


    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        _audioManager.PlaySound("RocketLaunch");
        Cursor.visible = false;
    }

    /// <summary>
    /// Stoppt den RocketLaunch Sound und Startet einen neuen SOund mit dem Namen "name"
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        _audioManager.StopSound("RocketLaunch");
        _audioManager.PlaySound(name);
    }
}
