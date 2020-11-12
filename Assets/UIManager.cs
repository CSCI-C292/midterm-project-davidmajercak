using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    float _levelTimer;

    void Awake() 
    {
        Instance = this;
        SceneManager.sceneLoaded += ResetLevelTimer;
    }

    void ResetLevelTimer(Scene scene, LoadSceneMode mode)
    {
        _levelTimer = 0;
    }
}
