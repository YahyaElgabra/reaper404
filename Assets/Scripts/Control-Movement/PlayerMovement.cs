using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool _isSecondRun = false;
    public bool _isTP = false;
    public bool _isGrav = false;

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
    private const float JumpScale = 22f;
    private const float WallJumpVertScale = 25f;
    private const float WallJumpHoriScale = 12f;
    
    private Vector3 _normalizedInputDirection;

    private const float maxSpeed = 8f;
    private const float maxRunningSpeed = 12f;
    float speedH = 2.0f;
    private const float maxVerticalSpeed = 9.81f;

    public Camera playerCamera;
    public float defaultFOV = 70f;
    public float sprintFOV = 100f;
    public float fovTransitionSpeed = 1f;

    public Vector3 gravityDirection = Vector3.down;
    private float _gravityStrength = 28f;
    GravityControl gravityControl;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        playerCamera = GetComponentInChildren<Camera>();
        playerCamera.fieldOfView = defaultFOV;
        gravityControl = GetComponent<GravityControl>();
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

        if (_isGrav && (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonUp("Fire4")))
        {
            gravityControl.RotateGravity(0);
        }
        else if (_isGrav && (Input.GetKeyDown(KeyCode.X) || Input.GetButtonUp("Fire5")))
        {
            gravityControl.RotateGravity(1);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(-transform.up * _gravityStrength, ForceMode.Acceleration);

        Quaternion userRot = Quaternion.AngleAxis(_yaw, transform.up);
        transform.rotation = userRot * transform.rotation;

        _yaw = 0.0f;

        _normalizedInputDirection = Vector3.Normalize(Vector3.Normalize(transform.forward * _fbInput + transform.right * _lrInput) - _prevNormalizedWallJumpHori);
        
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

            // capping vertical speed
            Vector3 velocity = _rigidbody.velocity;
            float upVelocity = Vector3.Dot(velocity, -gravityDirection);
            if (upVelocity > maxVerticalSpeed)
            {
                float difference = upVelocity - maxVerticalSpeed;
                velocity = velocity - difference * -gravityDirection;
                _rigidbody.velocity = velocity;
            }

            _userJumped = false;
            _isGrounded = false;
            _jumpDisabled = true;
            StartCoroutine(RegainJump());
        }
        //Vector3 velocity1 = _rigidbody.velocity;
        //float upVelocity1 = Vector3.Dot(velocity1, -_gravityDirection);
        //if (0 < upVelocity1 && upVelocity1 < 0.5)
        //{
         //   _rigidbody.AddForce(transform.up * JumpScale * -0.8f, ForceMode.VelocityChange);
        //}
        if (_userWallJumped)
        {
            _userWallJumped = false;
            _isOnWall = false;
            Vector3 _normalizedVectorToWall = Vector3.Normalize(_vectorToWall);
            if (gravityDirection.x != 0)
            {
                _normalizedVectorToWall.x = 0;
            }
            else if (gravityDirection.y != 0)
            {
                _normalizedVectorToWall.y = 0;
            }
            else if (gravityDirection.z != 0)
            {
                _normalizedVectorToWall.z = 0;
            }
            _prevNormalizedWallJumpHori = _normalizedVectorToWall;
            Vector3 velocity = _rigidbody.velocity;
            float upVelocity = Vector3.Dot(velocity, -gravityDirection);
            Vector3 _final = (transform.up * (WallJumpVertScale - upVelocity )) - (_normalizedVectorToWall * WallJumpHoriScale);
            _rigidbody.AddForce(_final, ForceMode.VelocityChange);
            velocity = _rigidbody.velocity;
            upVelocity = Vector3.Dot(velocity, -gravityDirection);
            if (upVelocity > maxVerticalSpeed)
            {
                float difference = upVelocity - maxVerticalSpeed;
                velocity = velocity - difference * -gravityDirection;
                _rigidbody.velocity = velocity;
            }
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
}
