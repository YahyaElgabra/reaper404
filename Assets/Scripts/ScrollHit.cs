using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollHit : MonoBehaviour
{
    public SpawnPlayer spawnPlayer;
    public UpdatePlayerInfo updatePlayerInfo;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Destroy(this.gameObject);
            
            spawnPlayer = FindObjectOfType<SpawnPlayer>();
            updatePlayerInfo = FindObjectOfType<UpdatePlayerInfo>();
            
            spawnPlayer.score += 1;
            updatePlayerInfo.UpdatePlayerScoreText(spawnPlayer.score);
        }
    }
}
