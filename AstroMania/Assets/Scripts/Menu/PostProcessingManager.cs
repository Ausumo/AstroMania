using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    private Volume _volume;

    [SerializeField]
    private VolumeProfile _highProfile;
    [SerializeField]
    private VolumeProfile _mediumProfile;
    [SerializeField]
    private VolumeProfile _lowProfile;

    public int graphicSettings;

    private void Awake()
    {
        _volume = gameObject.GetComponent<Volume>();
    }

    public void SetGraphics()
    {
        switch(graphicSettings)
        {
            case 0: 
                Low();
                break;

            case 1:
                Medium();
                break;

            case 2:
                High();
                break;
        }
    }

    private void Low()
    {
        _volume.profile = _lowProfile;
        graphicSettings = 0;
    }

    private void Medium()
    {
        _volume.profile = _mediumProfile;
        graphicSettings = 1;
    }

    private void High()
    {
        _volume.profile = _highProfile;
        graphicSettings = 2;
    }
}
