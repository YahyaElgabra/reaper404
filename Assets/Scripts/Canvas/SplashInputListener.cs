using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class splashInputListener : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float fadeDuration = 1.0f;
    void Start()
    {
        if (textMeshPro == null) return;
        Color color = textMeshPro.color;
        color.a = 0;
        textMeshPro.color = color;
        StartCoroutine(FadeIn());
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            Color color = textMeshPro.color;
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            textMeshPro.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Color finalColor = textMeshPro.color;
        finalColor.a = 1;
        textMeshPro.color = finalColor;
    }
}
