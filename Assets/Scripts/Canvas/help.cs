using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class help : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }
    void Start()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (_inputActions.Gameplay.Help.IsPressed())
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
