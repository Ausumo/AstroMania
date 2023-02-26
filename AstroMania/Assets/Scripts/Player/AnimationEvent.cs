using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] private Sound[] _sounds;

    private int _soundRandom;

    private string _soundName;

    [SerializeField]
    private GameObject _footRight;
    [SerializeField]
    private GameObject _footLeft;
    [SerializeField]
    private float _distance = 0.2f;
    [SerializeField]
    private GameObject _playerObj;
    [SerializeField]
    private GameObject _footPrint;

    public void FootstepsEvent(int foot)
    {
        _soundRandom = Random.Range(0, _sounds.Length);

        switch (_soundRandom)
        {
            case 0:
                _soundName = "footstep1";
                break;
            case 1:
                _soundName = "footstep2";
                break;
        }

        AudioManager.Instance.PlaySound(_soundName);

        switch (foot)
        {
            case 0:
                //Rechter Fuﬂabdruck

                break;
            case 1:
                //Linker Fuﬂabdruck

                break;
        }
    }
}
