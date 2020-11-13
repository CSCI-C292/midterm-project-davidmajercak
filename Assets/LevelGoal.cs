using System.Collections;
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
            //Important to call this before event otherwise flashing text is delayed at first
            //Slows down game after completing level
            Time.timeScale = .1f;

            GameEvents.InvokeLevelCompleted();

            _levelCompleteTMP = GameObject.FindGameObjectWithTag("LevelCompleteText").GetComponent<TextMeshProUGUI>();
            _levelCompleteText = SceneManager.GetActiveScene().name + "\nComplete!";
            _levelCompleteTMP.text = _levelCompleteText;

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
            _levelCompleteTMP.text = "You beat the demo!\nMore levels coming soon!";
            yield return new WaitForSeconds(3f);
            //Return CursorLockMode to normal
            Cursor.lockState = CursorLockMode.None;
            //Return to Main Menu
            SceneManager.LoadScene(0);
            //TODO
            //Need to reset the level completion text again before returning to main menu
            _levelCompleteTMP.text = "";
        }
        //If level isn't the last level
        else
        {
            //Load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
       
    }
}
