﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameState : MonoBehaviour
{
    float _lowestObjecty = Mathf.Infinity;
    float _highestObjecty = -Mathf.Infinity;
    float _lowestObjectx = Mathf.Infinity;
    float _highestObjectx = -Mathf.Infinity;
    float _lowestObjectz = Mathf.Infinity;
    float _highestObjectz = -Mathf.Infinity;
    [SerializeField] float _offset;
    [SerializeField] RuntimeData _runtimeData;

    public static GameState Instance;

    void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        //Reload scene if player presses R or if player goes too far out of bounds
        if (Input.GetKeyDown("r") ||
        _runtimeData.playerPosition.x > _highestObjectx || _runtimeData.playerPosition.x < _lowestObjectx ||
            _runtimeData.playerPosition.y > _highestObjecty || _runtimeData.playerPosition.y < _lowestObjecty ||
            _runtimeData.playerPosition.z > _highestObjectz || _runtimeData.playerPosition.z < _lowestObjectz)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //Revert any timescale changes (fixes bug if player resets just after completing level)
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Countdown();
        CalculateLevelBounds();
    }

    void Countdown()
    {
        _runtimeData.currentGameplayState = GameplayState.Countdown;

        StartCoroutine(EndCountdown());
    }

    IEnumerator EndCountdown()
    {
        yield return new WaitForSeconds(3f);
        _runtimeData.currentGameplayState = GameplayState.FreeMove;
    }

    //Checks the x, y, and z positions of every object in the scene to determine where the level bounds are
    //  This way we can auto-reload the scene if the player goes too far out of bounds
    void CalculateLevelBounds()
    {
        _lowestObjecty = Mathf.Infinity;
        _highestObjecty = -Mathf.Infinity;
        _lowestObjectx = Mathf.Infinity;
        _highestObjectx = -Mathf.Infinity;
        _lowestObjectz = Mathf.Infinity;
        _highestObjectz = -Mathf.Infinity;
        //Probably inefficient but works well enough for now
        //Get all gameObjects in scene
        GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        //Set min/max of x y and z based on the positions of the objects in the scene
        for(int i = 0; i < gameObjects.Length; i++)
        {
            if(gameObjects[i].transform.position.y > _highestObjecty)
            {
                _highestObjecty = gameObjects[i].transform.position.y;
            }
            if (gameObjects[i].transform.position.y < _lowestObjecty)
            {
                _lowestObjecty = gameObjects[i].transform.position.y;
            }

            if (gameObjects[i].transform.position.x > _highestObjectx)
            {
                _highestObjectx = gameObjects[i].transform.position.x;
            }
            if (gameObjects[i].transform.position.x < _lowestObjectx)
            {
                _lowestObjectx = gameObjects[i].transform.position.x;
            }

            if (gameObjects[i].transform.position.z > _highestObjectz)
            {
                _highestObjectz = gameObjects[i].transform.position.z;
            }
            if (gameObjects[i].transform.position.z < _lowestObjectz)
            {
                _lowestObjectz = gameObjects[i].transform.position.z;
            }
        }

        //Add the offset value to the min/max positions
        _lowestObjecty += -_offset;
        _highestObjecty += _offset;
        _lowestObjectx += -_offset;
        _highestObjectx += _offset;
        _lowestObjectz += -_offset;
        _highestObjectz += _offset;
    }

}