using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            if (SceneManager.GetActiveScene().name == "LevelSelect")
            {
                SceneManager.LoadScene("MainMenu");
            }
            else if (SceneManager.GetActiveScene().name == "MainMenu") 
            {
                SceneManager.LoadScene("SplashScreen");
            }
            else if (SceneManager.GetActiveScene().name == "SplashScreen")
            {
                return;
            }
            else
            {
                SceneManager.LoadScene("LevelSelect");
            }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void LoadLevel(string levelName)
    {
        Debug.Log("Loading Level: " + levelName);
        SceneManager.LoadScene(levelName);
    }
}