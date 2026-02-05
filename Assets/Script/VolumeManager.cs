using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioMixer audioMixer;

    void Start()
    {
        float volume = PlayerPrefs.GetFloat("MasterVolume", 3f);

        volumeSlider.value = volume;
        SetVolume(volume);

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float value)
    {
        // Convert 0–1 → decibel
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 3f)) * 80f;

        audioMixer.SetFloat("MasterVolume", dB);

        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }
}
