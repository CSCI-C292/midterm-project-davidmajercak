﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelGoal : MonoBehaviour
{
    TextMeshProUGUI _levelCompleteTMP;
    string _levelCompleteText;
    [SerializeField] int _lastLevel;
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            _levelCompleteTMP = GameObject.FindGameObjectWithTag("LevelCompleteText").GetComponent<TextMeshProUGUI>();
            _levelCompleteText = SceneManager.GetActiveScene().name + "\nComplete!";
            _levelCompleteTMP.text = _levelCompleteText;
            //Slows down game after completing level
            Time.timeScale = .1f;
            //Smooths out the slowing down of the game so it's not choppy
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            StartCoroutine(LevelComplete());
        }
    }

    IEnumerator LevelComplete()
    {
        //Wait for about 3 seconds after completing level
        yield return new WaitForSeconds(.3f);
        //Remove level completion text
        _levelCompleteTMP.text = "";
        //Revert the timescale changes
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;

        //If level is the last level
        if(SceneManager.GetActiveScene().buildIndex == _lastLevel)
        {
            //Demo completion message
            _levelCompleteTMP.text = "You beat the demo!\nStay tuned for more levels!";
            yield return new WaitForSeconds(3f);
            //Return CursorLockMode to normal
            Cursor.lockState = CursorLockMode.None;
            //Return to Main Menu
            SceneManager.LoadScene(0);
        }
        //If level isn't the last level
        else
        {
            //Load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
       
    }
}
