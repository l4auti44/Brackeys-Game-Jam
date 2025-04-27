using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    public static bool isGamePaused = false;
    public static bool isGameStopped = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public static void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

    public static void SceneLoader(string name)
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }


}