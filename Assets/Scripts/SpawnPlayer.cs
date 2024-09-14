using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{   
    private GameObject _player;
    private Transform _playerTransform;
    private Rigidbody _rigidbody;
    
    private Vector3 _startPosition = new Vector3(0, 11f, 0);
    private const float RespawnHeight = 0f;
    
    public int numLives = 3;
    public int livesLeft;
    
    public int startScore = 0;
    public int score;
    
    public UpdatePlayerInfo updatePlayerInfo;
    
    void Start()
    {
        _player = Instantiate(Resources.Load("Player"),
            _startPosition, Quaternion.identity) as GameObject;

        _playerTransform = _player.transform;
        _rigidbody = _player.GetComponent<Rigidbody>();
        
        livesLeft = numLives;
        score = startScore;
        updatePlayerInfo = FindObjectOfType<UpdatePlayerInfo>();
    }
    
    void Update()
    {
        if (_playerTransform.position.y < RespawnHeight)
        {
            if (livesLeft > 1)
            {
                _playerTransform.position = _startPosition;
                _rigidbody.velocity = Vector3.zero;
                
                livesLeft -= 1;
                updatePlayerInfo.UpdatePlayerLivesText(livesLeft);
            }
            else
            {
                Debug.Log("GameOver");
                updatePlayerInfo.UpdatePlayerLivesText(livesLeft-1);
                Time.timeScale = 0;
                updatePlayerInfo.DisplayGameOver();
            }
        }
    }
}
