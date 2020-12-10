using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
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

        GameEvents.SaveData += SaveData;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        GameEvents.LevelCompleted -= SaveData;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void SaveData(object sender, EventArgs args)
    { 
        //If level time for this level doesn't exist or if new level timer is lower than previous
        if(!PlayerPrefs.HasKey(SceneManager.GetActiveScene().name) ||
            PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name) > _runtimeData.levelTimer)
        {
            //Save new best time for this level
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, _runtimeData.levelTimer);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(UIManager.Instance.CheckIfPreviousBestExists())
        {
            UIManager.Instance._previousBestTMP.text = "Previous Best: " + PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name).ToString("F2");
        }
    }


}
