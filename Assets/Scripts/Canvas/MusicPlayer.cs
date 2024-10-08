using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer musicPlayer;

    void Start()
    {
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
}
