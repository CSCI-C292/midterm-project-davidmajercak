using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Player _player;
    [SerializeField] Slider _volumeSlider;
    [SerializeField] Slider _lookSensitivitySlider;

    void Start()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.25f);
        _lookSensitivitySlider.value = PlayerPrefs.GetFloat("lookSensitivity", 3f);
    }
    public void SetVolume(float volume)
    {
        _audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void SetLookSensitivity(float lookSensitivity)
    {
        _player._lookSensitivity = lookSensitivity;
        PlayerPrefs.SetFloat("lookSensitivity", lookSensitivity);
    }
}
