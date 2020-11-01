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
            StartCoroutine(LevelComplete());
        }
    }

    IEnumerator LevelComplete() 
    {
        yield return new WaitForSeconds(3);
        _levelCompleteTMP.text = "";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
