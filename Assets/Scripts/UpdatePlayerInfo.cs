using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerInfo : MonoBehaviour
{
    private GameObject _playerLivesObj;
    public GameObject gameOverObj;
    private GameObject _playerScoreObj;
    private GameObject _bossLivesObj;
    public GameObject winObj;
    
    private Text _playerLivesText;
    private Text _playerScoreText;
    private Text _bossLivesText;
    
    public SpawnPlayer spawnPlayer;
    public SpawnBoss spawnBoss;
    
    void Start()
    {
        gameOverObj.SetActive(false);
        winObj.SetActive(false);
        
        spawnPlayer = FindObjectOfType<SpawnPlayer>();
        spawnBoss = FindObjectOfType<SpawnBoss>();
        
        _playerLivesObj = GameObject.Find("Lives");
        _playerLivesText = _playerLivesObj.GetComponent<Text>();
        _playerLivesText.text = "Lives: " + spawnPlayer.numLives;
        
        _playerScoreObj = GameObject.Find("Score");
        _playerScoreText = _playerScoreObj.GetComponent<Text>();
        _playerScoreText.text = "Scrolls: " + spawnPlayer.startScore;
        
        _bossLivesObj = GameObject.Find("BossLives");
        _bossLivesText = _bossLivesObj.GetComponent<Text>();
        _bossLivesText.text = "Boss Lives: " + spawnBoss.numBossLives;
    }

    
    public void UpdatePlayerLivesText(int livesLeft) 
    {
        _playerLivesText.text = "Lives: " + livesLeft;
    }

    public void DisplayGameOver()
    {
        gameOverObj.SetActive(true);
    }

    public void UpdatePlayerScoreText(int score)
    {
        _playerScoreText.text = "Scrolls: " + score;
    }
    
    public void UpdateBossLivesText(int bossLivesLeft) 
    {
        _bossLivesText.text = "Boss Lives: " + bossLivesLeft;
    }
    
    public void DisplayWin()
    {
        winObj.SetActive(true);
    }
}
