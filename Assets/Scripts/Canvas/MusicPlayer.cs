using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer musicPlayer;
    public Slider volumeSlider;

    void Start()
    {
        LoadVolume();
        if (musicPlayer == null)
        {
            musicPlayer = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (gameObject.GetComponent<AudioSource>().clip == musicPlayer.GetComponent<AudioSource>().clip)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(musicPlayer.gameObject);
                musicPlayer = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (volumeSlider != null) return;
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null) return;
        Transform audioSliderTransform = canvas.transform.Find("audio slider");
        if (audioSliderTransform == null) return;
        Transform sliderTransform = audioSliderTransform.Find("Slider");
        if (sliderTransform == null) return;
        Slider slider = sliderTransform.GetComponent<Slider>();
        if (slider == null) return;
        volumeSlider = slider;
    }

    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }

    public void LoadVolume()
    {
        if (volumeSlider == null)
        {
            return;
        }

        if (PlayerPrefs.HasKey("volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
        else
        {
            PlayerPrefs.SetFloat("volume", 1);
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
    }
}
