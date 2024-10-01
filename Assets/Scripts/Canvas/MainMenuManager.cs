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
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void LoadLevel(string levelName)
    {
        Debug.Log("Loading Level: " + levelName);
        SceneManager.LoadScene(levelName);
    }
}