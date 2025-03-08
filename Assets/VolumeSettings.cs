using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        if (MusicSlider == null || SFXSlider == null || audioMixer == null)
        {
            Debug.Log(MusicSlider);
            Debug.Log(SFXSlider);
            Debug.Log(audioMixer);

            Debug.LogError("One or more references are missing in VolumeSettings! Please assign them in the Inspector.");
            return;
        }

        if (PlayerPrefs.HasKey("Music"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        if (MusicSlider == null || audioMixer == null) return;

        float volume = MusicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("Music", volume);
    }

    public void SetSFXVolume()
    {
        if (SFXSlider == null || audioMixer == null) return;

        float volume = SFXSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
    }

    public void LoadVolume()
    {
        if (MusicSlider != null) MusicSlider.value = PlayerPrefs.GetFloat("Music", 1.0f);
        if (SFXSlider != null) SFXSlider.value = PlayerPrefs.GetFloat("SFX", 1.0f);

        SetMusicVolume();
        SetSFXVolume();
    }
}