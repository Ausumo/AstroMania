using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingManager : MonoBehaviour
{
    //Instance
    private static PostProcessingManager _instance;
    public static PostProcessingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PostProcessingManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private Volume _volumeProfile;

    private void Start()
    {
        _volumeProfile = gameObject.GetComponent<Volume>();
    }

    public int SetGraphics(int setting)
    {
        switch(setting)
        {
            case 0:

                break;

            case 1:

                break;

            case 2:

                break;
        }

        return -1;
    }

    private void Low()
    {

    }

    private void Medium()
    {

    }

    private void High()
    {

    }
}
