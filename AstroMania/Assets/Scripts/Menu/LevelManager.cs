using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private bool _isOnTesting;

    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        if(!_isOnTesting)
            _audioManager = GameObject.FindGameObjectWithTag("AudioManager").gameObject.GetComponent<AudioManager>();
    }

    public void PlaySound(string soundName)
    {
        if (!_isOnTesting)
            _audioManager.PlaySound(soundName);
    }

    public void PlayMusic(string musicName)
    {
        if (!_isOnTesting)
            _audioManager.PlayMusic(musicName);
    }
}
