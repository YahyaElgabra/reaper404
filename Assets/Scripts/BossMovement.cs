using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Transform playerTransform;
    private GameObject _player;
    
    private Rigidbody _rigidbody;
    private const float MoveSpeed = 1.5f * 4;
    private const float StoppingDistance = 1.5f;

    private float _distanceToPlayer;
    private const float MaxDistance = 15f;
    
    private Vector3 _newPos;
    private const float RotSpeed = 5f;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = _player.transform;
        
        _distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        if (_distanceToPlayer < StoppingDistance)
        {
            return;
        }
        
        if (_distanceToPlayer < MaxDistance)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotSpeed * Time.deltaTime);
            
            _newPos = transform.position + direction * (MoveSpeed * Time.deltaTime);
            _rigidbody.MovePosition(_newPos);
        }
    }
}
