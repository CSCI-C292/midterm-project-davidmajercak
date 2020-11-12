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
    [SerializeField] Image _crosshair;
    [SerializeField] Color _canGrappleColor;
    [SerializeField] Color _cannotGrappleColor;
    float _levelTimer;
    bool _isLevelCompleted;
    [SerializeField] RuntimeData _runtimeData;

    void Awake() 
    {
        Instance = this;

        SceneManager.sceneLoaded += ResetLevelTimer;
        GameEvents.LevelCompleted += LevelCompleted;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= ResetLevelTimer;
        GameEvents.LevelCompleted -= LevelCompleted;
    }

    void Start()
    {
        _isLevelCompleted = false;
        _crosshair = _crosshair.GetComponent<Image>();
        _levelTimer = 0;
    }


    void Update()
    {
        SetCrosshairColor();

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

        StartCoroutine(FlashLevelTimerText());
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

}
