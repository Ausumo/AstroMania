using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class CutsceneManager : MonoBehaviour
{

    private AudioManager _audioManager;

    [SerializeField] private GameObject _backToMainMenuButtom;


    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        _audioManager.PlaySound("RocketLaunch");
    }


    public void PlaySound(string name)
    {
        _audioManager.StopSound("RocketLaunch");
        _audioManager.PlaySound(name);
    }

    public void ShowButton()
    {
        _backToMainMenuButtom.SetActive(true);
    }
}
