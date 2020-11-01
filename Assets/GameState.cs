using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{

    public static GameState Instance;

    void Update()
    {

        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //Revert any timescale changes (fixes bug if player resets just after completing level)
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }
    }
}