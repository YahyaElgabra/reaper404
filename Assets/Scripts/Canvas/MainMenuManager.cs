using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    public GameObject help;
    GameObject winScreen;
    void Awake()
    {
        _inputActions = new PlayerInputActions();
        if (help != null)
        {
            help.SetActive(false);
        }
    }

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("WinScreen"))
            {
                winScreen = child.gameObject;
                break;
            }
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
        if (winScreen == null)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("WinScreen"))
                {
                    winScreen = child.gameObject;
                    break;
                }
            }
        }
        else if (winScreen.activeSelf && _inputActions.Gameplay.Jump.IsPressed())
        {
            winScreen.SetActive(false);
            List<string> levels = LevelSelectPicker.levelOrder;
            int nextIndex = levels.IndexOf(SceneManager.GetActiveScene().name) + 1;
            SceneManager.LoadScene(levels[nextIndex]);
        }
    }
}