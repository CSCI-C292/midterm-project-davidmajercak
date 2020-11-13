using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager Instance;
    AudioSource _audioSource;
    [SerializeField] AudioClip[] _trackList;
    [SerializeField] RuntimeData _runtimeData;

    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            _audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
            Random rng = new Random();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ShuffleSongs();
        StartCoroutine(PlayNextSong());
    }

    IEnumerator PlayNextSong()
    {
        for(int i = 0; i < _trackList.Length; i++)
        {
            _audioSource.clip = _trackList[i];
            _audioSource.Play();
            
            _runtimeData.songName = _trackList[i].ToString();
            GameEvents.InvokeSongStarted();

            yield return new WaitForSeconds(_trackList[i].length);
        }
        
        ShuffleSongs();
        PlayNextSong();
    }

    void ShuffleSongs()
    {
        for (int t = 0; t < _trackList.Length; t++)
        {
            AudioClip tmp = _trackList[t];
            int r = Random.Range(t, _trackList.Length);
            _trackList[t] = _trackList[r];
            _trackList[r] = tmp;
        }
    }
}
