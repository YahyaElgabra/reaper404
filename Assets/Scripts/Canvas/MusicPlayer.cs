using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
