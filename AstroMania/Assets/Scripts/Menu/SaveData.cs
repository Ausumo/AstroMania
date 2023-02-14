[System.Serializable]
public class OptionValues
{
    public float masterVolume;
    public float musicVolume;
    public float soundVolume;

    public OptionValues(float masterVolume, float musicVolume, float soundVolume)
    {
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.soundVolume = soundVolume;
    }
}
