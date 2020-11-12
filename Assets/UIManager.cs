using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] TextMeshProUGUI _levelTimer;


    void Awake() 
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {

    }


    void Update()
    {
        _levelTimer.text = Time.timeSinceLevelLoad.ToString("F2");
    }
}
