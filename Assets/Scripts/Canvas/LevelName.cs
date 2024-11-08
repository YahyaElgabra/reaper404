using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelName : MonoBehaviour
{
    void Start()
    {
        TextMeshProUGUI textComponent = GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = SceneManager.GetActiveScene().name;
    }
}
