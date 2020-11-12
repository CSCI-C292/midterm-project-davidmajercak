using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] TextMeshProUGUI _levelTimerTMP;
    float _levelTimer;
    bool _isLevelCompleted;

    void Awake() 
    {
        Instance = this;
        //Keep this object between scenes
        DontDestroyOnLoad(this.gameObject);
        //Keep this object's parent (Canvas) in between scenes
        Canvas canvas = gameObject.GetComponentInParent<Canvas>();
        DontDestroyOnLoad(canvas);

        SceneManager.sceneLoaded += ResetLevelTimer;
        GameEvents.LevelCompleted += LevelCompleted;
    }

    void Start()
    {
        _isLevelCompleted = false;
    }


    void Update()
    {
        //Only increment level time if level is not completed
        if(!_isLevelCompleted)
            _levelTimer += Time.deltaTime;
        _levelTimerTMP.text = _levelTimer.ToString("F2");
    }

    void ResetLevelTimer(Scene scene, LoadSceneMode mode)
    {
        _levelTimer = 0;
        _isLevelCompleted = false;
    }

    void LevelCompleted(object sender, EventArgs args)
    {
        _isLevelCompleted = true;
    }

}
