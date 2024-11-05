using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    public GameObject help;
    void Awake()
    {
        _inputActions = new PlayerInputActions();
        if (help != null)
        {
            help.SetActive(false);
        }
    }

    void OnEnable()
    {
        _inputActions.Gameplay.Enable();
    }

    void OnDisable()
    {
        _inputActions.Gameplay.Disable();
    }
    
    private void Update()
    {
        if (_inputActions.Gameplay.Escape.IsPressed())
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
        if (_inputActions.Gameplay.Help.WasPressedThisFrame())
        {
            if (help != null)
            {
                help.SetActive(!help.activeSelf);
            }
        }
    }

    public void LoadLevel(string levelName)
    {
        Debug.Log("Loading Level: " + levelName);
        SceneManager.LoadScene(levelName);
    }
}