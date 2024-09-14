using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    public int numBossLives = 3;
    public int bossLivesLeft;
    
    public UpdatePlayerInfo updatePlayerInfo;
    
    void Start()
    {
        bossLivesLeft = numBossLives;
    }
    
    void Update()
    {
        updatePlayerInfo = FindObjectOfType<UpdatePlayerInfo>();
        
        if (bossLivesLeft <= 0)
        {
            Time.timeScale = 0;
            Debug.Log("YouWin");
            updatePlayerInfo.DisplayWin();
        }
        
    }
}