using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer musicPlayer;
    private AudioClip clip;

    void Start()
    {
        if (musicPlayer == null)
        {
            musicPlayer = this;
            clip = gameObject.GetComponent<AudioSource>().clip;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (clip == gameObject.GetComponent<AudioSource>().clip)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(musicPlayer.gameObject);
                musicPlayer = this;
                clip = gameObject.GetComponent<AudioSource>().clip;
                DontDestroyOnLoad(gameObject);
            }
        }
        
    }
}
