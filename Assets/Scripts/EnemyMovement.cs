using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTransform;
    private GameObject _player;
    
    private Rigidbody _rigidbody;
    private const float MoveSpeed = 5f * 3;
    
    private float _xDistanceToPlayer;
    private float _zDistanceToPlayer;
    private const float MinDistanceX = 0.1f;
    private const float MaxDistanceZ = 10f;
    
    private Vector3 _newPos;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = _player.transform;
        
        _xDistanceToPlayer = playerTransform.position.x - transform.position.x;
        _zDistanceToPlayer = playerTransform.position.z - transform.position.z;
        
        if ((Mathf.Abs(_xDistanceToPlayer) > MinDistanceX) && (Mathf.Abs(_zDistanceToPlayer) < MaxDistanceZ))
        {
            _newPos = new Vector3(transform.position.x + _xDistanceToPlayer * MoveSpeed * Time.deltaTime,
                transform.position.y, transform.position.z);
            _rigidbody.MovePosition(_newPos);
        }
    }
}
