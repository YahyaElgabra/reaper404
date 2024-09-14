using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public Transform playerTransform;
    private GameObject _player;
    
    private const float AttackRange = 5f;
    private const float AttackDelay = 3f;
    private const float PushForce = 20f;

    private bool _isAttacking = false;
    private Vector3 _pushDirection;
    
    public UpdatePlayerInfo updatePlayerInfo;
    public SpawnPlayer spawnPlayer;

    private void Start()
    {
        updatePlayerInfo = FindObjectOfType<UpdatePlayerInfo>();
        spawnPlayer = FindObjectOfType<SpawnPlayer>();
    }

    void Update()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = _player.transform;
        
        if (Vector3.Distance(transform.position, playerTransform.position) <= AttackRange)
        {
            if (!_isAttacking)
            {
                StartCoroutine(AttackAfterDelay());
            }
        }
    }

    private IEnumerator AttackAfterDelay()
    {
        _isAttacking = true;
        yield return new WaitForSeconds(AttackDelay);
        
        if (Vector3.Distance(transform.position, playerTransform.position) <= AttackRange)
        {
            PushPlayer();
        }
        
        _isAttacking = false;
    }

    void PushPlayer()
    {
        _pushDirection = (playerTransform.position - transform.position).normalized;
        
        if (playerTransform.TryGetComponent(out Rigidbody playerRigidbody))
        {
            playerRigidbody.AddForce(_pushDirection * PushForce, ForceMode.Impulse);
        }
        
        if (spawnPlayer.livesLeft > 1)
        {
            spawnPlayer.livesLeft -= 1;
            updatePlayerInfo.UpdatePlayerLivesText(spawnPlayer.livesLeft);
        }
        else
        {
            Debug.Log("GameOver");
            updatePlayerInfo.UpdatePlayerLivesText(spawnPlayer.livesLeft-1);
            Time.timeScale = 0;
            updatePlayerInfo.DisplayGameOver();
        }
    }
}
