using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] TextMeshProUGUI _levelTimerTMP;
    [SerializeField] TextMeshProUGUI _artistNameTMP;
    [SerializeField] TextMeshProUGUI _songNameTMP;
    [SerializeField] TextMeshProUGUI _levelCompletedTMP;
    [SerializeField] TextMeshProUGUI _countdownTMP;
    [SerializeField] TextMeshProUGUI _playerVelocityTMP;
    [SerializeField] TextMeshProUGUI _levelNameTMP;
    [SerializeField] public TextMeshProUGUI _previousBestTMP;
    [SerializeField] Image _crosshair;
    [SerializeField] Color _canGrappleColor;
    [SerializeField] Color _cannotGrappleColor;
    float _levelTimer;
    float _countdownTimer;
    float _pauseLevelTimer;
    [SerializeField] Color _defaultLevelTimerColor;
    [SerializeField] Color _pausedLevelTimerColor;
    bool _isLevelCompleted;
    [SerializeField] RuntimeData _runtimeData;

    void Awake() 
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEvents.LevelCompleted += LevelCompleted;
        GameEvents.SongStarted += DisplaySongInformation;
        GameEvents.GatheredCollectible += OnGatheredCollectible;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEvents.LevelCompleted -= LevelCompleted;
        GameEvents.SongStarted -= DisplaySongInformation;
        GameEvents.GatheredCollectible -= OnGatheredCollectible;
    }

    void Start()
    {
        _isLevelCompleted = false;
        _crosshair = _crosshair.GetComponent<Image>();
        _levelTimer = 0;
        _pauseLevelTimer = 0;
        _countdownTimer = 3;
        _playerVelocityTMP.text = _runtimeData.playerVelocity.ToString("F2") + " m/s";
    }


    void Update()
    {
        SetCrosshairColor();

        //Only increment level time if level is not completed
        if(!_isLevelCompleted)
        {
            if(_runtimeData.currentGameplayState == GameplayState.Countdown)
            {
                _runtimeData.playerVelocity = 0;
                _countdownTimer -= Time.deltaTime;
                
                if(_countdownTimer <= 0)
                    _countdownTMP.enabled = false;

                _countdownTMP.text = _countdownTimer.ToString("F2");
            }
            else
            {
                //Disable countdown timer and previous best message
                _countdownTMP.enabled = false;
                _previousBestTMP.enabled = false;
                

                if (_pauseLevelTimer > 0)
                {
                    _pauseLevelTimer -= Time.deltaTime;
                    _levelTimerTMP.color = _pausedLevelTimerColor;
                }
                else
                {
                    _levelTimer += Time.deltaTime;
                    _levelTimerTMP.color = _defaultLevelTimerColor;
                }
            }

            _playerVelocityTMP.text = _runtimeData.playerVelocity.ToString("F2") + " m/s";
        }

        _levelTimerTMP.text = _levelTimer.ToString("F2");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetLevelTimer();

        _countdownTimer = 3;
        _pauseLevelTimer = 0;
        _levelCompletedTMP.text = "";
        _levelTimerTMP.color = _defaultLevelTimerColor;
        _levelTimerTMP.alpha = 255;
        _levelNameTMP.text = SceneManager.GetActiveScene().name.Substring(SceneManager.GetActiveScene().name.IndexOf("-") + 1);
        

        //These are to keep Song text on main menu but hide everything else (and to click through them)
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            _crosshair.enabled = false;
            _levelTimerTMP.enabled = false;
            _levelCompletedTMP.enabled = false;
            _countdownTMP.enabled = false;
            _playerVelocityTMP.enabled = false;
            _levelNameTMP.enabled = false;
            _previousBestTMP.enabled = false;
        }
        else 
        {
            _crosshair.enabled = true;
            _levelTimerTMP.enabled = true;
            _levelCompletedTMP.enabled = true;
            _countdownTMP.enabled = true;
            _playerVelocityTMP.enabled = true;
            _levelNameTMP.enabled = true;

            if(CheckIfPreviousBestExists())
            {
                _previousBestTMP.enabled = true;
            }
            else
            {
                _previousBestTMP.enabled = false;
            }
        }
    }

    public Boolean CheckIfPreviousBestExists()
    {
        return PlayerPrefs.HasKey(SceneManager.GetActiveScene().name);
    }
    void ResetLevelTimer()
    {
        _levelTimer = 0;
        _isLevelCompleted = false;
    }

    void LevelCompleted(object sender, EventArgs args)
    {
        SendInfoToSaveManager();

        _isLevelCompleted = true;

        StartCoroutine(FlashLevelTimerText());
    }

    void SendInfoToSaveManager()
    {
        _runtimeData.levelTimer = _levelTimer;
        GameEvents.InvokeSaveData();
    }

    IEnumerator FlashLevelTimerText()
    {
        while(_isLevelCompleted)
        {
            _levelTimerTMP.alpha = 0;
            yield return new WaitForSeconds(.1f * Time.timeScale);
            _levelTimerTMP.alpha = 255;
            yield return new WaitForSeconds(.2f * Time.timeScale);
        }
    }

    void SetCrosshairColor()
    {
        if(_runtimeData.canGrapple)
        {
            _crosshair.color = _canGrappleColor;
        }
        else
        {
            _crosshair.color = _cannotGrappleColor;
        }
    }

    //Shows the artist and song name on screen after a new song is played
    void DisplaySongInformation(object sender, EventArgs args)
    {
        //TODO
        //TricksnTraps is hard coded right now, probably need a scritpable object for songs later
        _artistNameTMP.text = "TricksnTraps";
        _songNameTMP.text = _runtimeData.songName.Substring(0, _runtimeData.songName.IndexOf("("));

        _artistNameTMP.enabled = true;
        _songNameTMP.enabled = true;
        _artistNameTMP.alpha = 255;
        _songNameTMP.alpha = 255;

        StartCoroutine(SongInformationAlpha());
    }

    //Fades the artist name and song name after a few seconds
    IEnumerator SongInformationAlpha()
    {
        yield return new WaitForSeconds(3f * Time.timeScale);

        _artistNameTMP.CrossFadeAlpha(.001f, 3, true);
        _songNameTMP.CrossFadeAlpha(.001f, 3, true);

        yield return new WaitForSeconds(3.1f * Time.timeScale);
        _artistNameTMP.enabled = false;
        _songNameTMP.enabled = false;
    }

    void OnGatheredCollectible(object sender, CollectibleEventArgs args)
    {
        _pauseLevelTimer += args.collectiblePayload;
    }
}
