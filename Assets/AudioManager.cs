using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioManager Instance;
    AudioSource _audioSource;
    [SerializeField] AudioClip[] _trackList;

    void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(PlayNextSong());
    }

    IEnumerator PlayNextSong()
    {
        Debug.Log("music");
        _audioSource.clip = _trackList[2];
        _audioSource.Play();
        yield return new WaitForSeconds(_trackList[2].length);
        StartCoroutine(PlayNextSong());
    }
}
