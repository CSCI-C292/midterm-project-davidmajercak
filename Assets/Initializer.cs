using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Initializer : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Player _player;
    
    void Start()
    {
        _audioMixer.SetFloat("volume", Mathf.Log10(PlayerPrefs.GetFloat("volume", 0.25f)) * 20);
        _player._lookSensitivity = PlayerPrefs.GetFloat("lookSensitivity", 3f);
    }
}
