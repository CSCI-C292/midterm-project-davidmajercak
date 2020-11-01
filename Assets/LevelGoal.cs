using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelGoal : MonoBehaviour
{
    TextMeshProUGUI _levelCompleteTMP;
    string _levelCompleteText;
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
        yield return new WaitForSeconds(3);
        //Wait for about 3 seconds after completing level
        yield return new WaitForSeconds(.3f);
        //Remove level completion text
        _levelCompleteTMP.text = "";
        //Revert the timescale changes
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        //Load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
