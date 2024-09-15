using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool _isSecondRun = false;

    private float _fbInput;
    private float _lrInput;
    private float _rotationInput;
    
    private bool _isGrounded;
    private bool _isOnWall;
    private bool _userJumped;
    private bool _userWallJumped;
    private bool _running = false;

    private Vector3 _wallJump = new Vector3(0.0f, 7.0f, 0.0f);

    private Rigidbody _rigidbody;

    private const float MoveScale = 25f;
    private const float RunScale = 35f;
    private const float RotScale = 2f;
    private const float JumpScale = 8f;

    private Vector3 _normalizedInputDirection;

    private const float maxSpeed = 8f;
    private const float maxRunningSpeed = 12f;
    float speedH = 2.0f;
    float yaw = 0.0f;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        _fbInput = Input.GetAxisRaw("Vertical");
        _lrInput = Input.GetAxisRaw("Horizontal");


        yaw += speedH* Input.GetAxis("Mouse X");

        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        if (Input.GetKey(KeyCode.LeftShift) && _isSecondRun)
        {
            _running = true;
        }
        else
        {
            _running = false;
        }

        if (Input.GetButton("Jump")){
            if (_isGrounded)
            {
                _userJumped = true;
            }
            else if (_isOnWall && _isSecondRun) 
            {
                _userWallJumped = true;
            }
            
        }
    }

    private void FixedUpdate()
    {
        Vector3 userRot = transform.rotation.eulerAngles + new Vector3(0, _rotationInput * RotScale, 0);
        transform.rotation = Quaternion.Euler(userRot);

        _normalizedInputDirection = Vector3.Normalize(transform.forward * _fbInput + transform.right * _lrInput);

        Vector3 _currentMovementWithoutVertical = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        float _currentMax = _running ? maxRunningSpeed : maxSpeed;
        if (_currentMax <= _currentMovementWithoutVertical.magnitude)
        {
            Debug.Log("hit max");
            if (_rigidbody.velocity.x > 0)
            {
                _normalizedInputDirection.x = Mathf.Min(_normalizedInputDirection.x, 0);
            }
            if (_rigidbody.velocity.x < 0)
            {
                _normalizedInputDirection.x = Mathf.Max(_normalizedInputDirection.x, 0);
            }
            if (_rigidbody.velocity.z > 0)
            {
                _normalizedInputDirection.z = Mathf.Min(_normalizedInputDirection.z, 0);
            }
            if (_rigidbody.velocity.z < 0)
            {
                _normalizedInputDirection.z = Mathf.Max(_normalizedInputDirection.z, 0);
            }
        }
        if (!_running)
        {
            _rigidbody.AddForce(_normalizedInputDirection * MoveScale, ForceMode.Force);
        }
        else {
            _rigidbody.AddForce(_normalizedInputDirection * RunScale, ForceMode.Force);
        }

        if (_userJumped)
        {
            _rigidbody.AddForce(Vector3.up * JumpScale, ForceMode.VelocityChange);
            _userJumped = false;
            _isGrounded = false;
        }
        if (_userWallJumped)
        {
            _userWallJumped = false;
            _isOnWall = false;
            _rigidbody.AddForce(_wallJump, ForceMode.VelocityChange);

        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        _isGrounded = true;
    }

    private void OnTriggerEnter(Collider trigger)
    {
        _isOnWall = true;
    }

    private void OnTriggerExit(Collider trigger)
    {
        _isOnWall = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
    }
}
