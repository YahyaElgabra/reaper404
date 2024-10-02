using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool _isSecondRun = false;

    private float _fbInput;
    private float _lrInput;
    private float _yaw;
    
    private bool _isGrounded;
    private bool _isOnWall;
    private bool _userJumped;
    private bool _userWallJumped;
    private bool _jumpDisabled = false;
    private bool _running = false;
    private Vector3 _vectorToWall;
    private Vector3 _prevNormalizedWallJumpHori = Vector3.zero;

    private Rigidbody _rigidbody;

    private const float MoveScale = 25f;
    private const float RunScale = 35f;
    private const float RotScale = 2f;
    private const float JumpScale = 9f;
    private const float WallJumpVertScale = 14f;
    private const float WallJumpHoriScale = 12f;
    
    private Vector3 _normalizedInputDirection;

    private const float maxSpeed = 8f;
    private const float maxRunningSpeed = 12f;
    float speedH = 2.0f;

    public Camera playerCamera;
    public float defaultFOV = 70f;
    public float sprintFOV = 100f;
    public float fovTransitionSpeed = 1f;

    public Vector3 _gravityDirection = Vector3.down;
    private float _gravityStrength = 9.81f;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        playerCamera = GetComponentInChildren<Camera>();
        playerCamera.fieldOfView = defaultFOV;
    }

    public void SetIsGrounded(bool grounded)
    {
        _isGrounded = grounded;
    }

    public void SetIsOnWall(bool onWall, Vector3 closestPointOnWall)
    {
        _isOnWall = onWall;
        _vectorToWall = closestPointOnWall;
    }

    void Update()
    {
        _fbInput = Input.GetAxisRaw("Vertical");
        _lrInput = Input.GetAxisRaw("Horizontal");

        
        if (!Throwing.isAiming)
        {
            _yaw += speedH* (Input.GetAxis("Mouse X") + Input.GetAxis("RJoy X"));
        }
        else
        {
            _yaw = 0.0f;
        }
        
        // if ((Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire2")))
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire2")) && _isSecondRun)
        {
            _running = true;
        }
        else
        {
            _running = false;
        }
        // Debug.Log(_rigidbody.velocity.z);
        
        float targetFOV = _running ? sprintFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
        
        if (Input.GetButton("Jump") && !_jumpDisabled){
            if (_isGrounded && !_userWallJumped)
            {
                _userJumped = true;
                _isGrounded = false;
            }
            else if (_isOnWall && _isSecondRun && !_userJumped) 
            {
                _userWallJumped = true;
                _isOnWall = false;
            }
        }

        // I'm keeping these here for now in case you want to use them for development purposes
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RotateGravity(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RotateGravity(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RotateGravity(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RotateGravity(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            RotateGravity(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            RotateGravity(5);
        }*/
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(-transform.up * _gravityStrength, ForceMode.Acceleration);

        Quaternion userRot = Quaternion.AngleAxis(_yaw, transform.up);
        transform.rotation = userRot * transform.rotation;

        _yaw = 0.0f;

        _normalizedInputDirection = Vector3.Normalize(Vector3.Normalize(transform.forward * _fbInput + transform.right * _lrInput) - _prevNormalizedWallJumpHori);
        
        // Ignore this
        // Vector3 movementDirection = new Vector3(_fbInput, 0, _lrInput);
        // if (movementDirection.magnitude > 0.5f)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(_normalizedInputDirection);
        //     // transform.rotation = targetRotation;
        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1f);
        // }
        
        Vector3 _currentMovementWithoutVertical = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        float _currentMax = _running ? maxRunningSpeed : maxSpeed;
        if (_currentMax <= _currentMovementWithoutVertical.magnitude)
        {
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
            _rigidbody.AddForce(transform.up * JumpScale, ForceMode.VelocityChange);
            _userJumped = false;
            _isGrounded = false;
            _jumpDisabled = true;
            StartCoroutine(RegainJump());
        }
        if (_userWallJumped)
        {
            _userWallJumped = false;
            _isOnWall = false;
            Vector3 _normalizedVectorToWall = Vector3.Normalize(_vectorToWall);
            if (_gravityDirection.x != 0)
            {
                _normalizedVectorToWall.x = 0;
            }
            else if (_gravityDirection.y != 0)
            {
                _normalizedVectorToWall.y = 0;
            }
            else if (_gravityDirection.z != 0)
            {
                _normalizedVectorToWall.z = 0;
            }
            _prevNormalizedWallJumpHori = _normalizedVectorToWall;
            Vector3 _final = (transform.up * WallJumpVertScale) - (_normalizedVectorToWall * WallJumpHoriScale);
            _rigidbody.AddForce(_final, ForceMode.VelocityChange);
            // Debug.Log(_final.ToString());
            _jumpDisabled = true;
            StartCoroutine(RegainJump());
            StartCoroutine(RegainFullHoriControl());

        }
    }
    
    private IEnumerator RegainJump()
    {
        yield return new WaitForSeconds(0.1f);
        _jumpDisabled = false;
    }

    private IEnumerator RegainFullHoriControl()
    {
        yield return new WaitForSeconds(0.5f);
        _prevNormalizedWallJumpHori = Vector3.zero;
    }

        public void RotateGravity(int side)
    {
        switch (side)
        {
            case 0:
                _gravityDirection = new Vector3(1, 0, 0);
                break;
            case 1:
                _gravityDirection = new Vector3(-1, 0, 0);
                break;
            case 2:
                _gravityDirection = new Vector3(0, 1, 0);
                break;
            case 3:
                _gravityDirection = new Vector3(0, -1, 0);
                break;
            case 4:
                _gravityDirection = new Vector3(0, 0, 1);
                break;
            case 5:
                _gravityDirection = new Vector3(0, 0, -1);
                break;
            default:
                break;
        }
        FlipCharacterModel();
    }

    private void FlipCharacterModel()
    {
        Quaternion rotation = Quaternion.FromToRotation(-transform.up, _gravityDirection);
        transform.rotation = rotation * transform.rotation;
    }
}
