using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ability boolean flags
    public bool _isRunWallJump = false;
    public bool _isTP = false;
    public bool _isGrav = false;
    public bool _isFly = false;

    // flying reaper prefab
    public GameObject flyingReaperPrefab;

    private float _fbInput;
    private float _lrInput;
    private float _yaw;
    
    private bool _isGrounded;
    private bool _isOnWall;
    private bool _userJumped;
    private bool _userWallJumped;
    private bool _isHoggingJump = false;
    private bool _jumpDisabled = false;
    public bool _running = false;
    private Vector3 _vectorToWall;
    private Vector3 _prevNormalizedWallJumpHori = Vector3.zero;

    private Rigidbody _rigidbody;

    private const float MoveScale = 40f;
    private const float maxSpeed = 12f;
    private const float RunScale = 40f;
    private const float maxRunningSpeed = 16f;
    //private const float _fakeDrag = 30f;
    // Value for force-based drag. We are not using that, we are using velocity-based drag instead. For that, drag must be between 0 and 1.
    private const float _groundDrag = 0.8f;
    private const float _airDrag = 0.05f;
    private const float _groundMultiplier = 1f;
    private const float _minimumSpeedForAirDrag = 0.5f;


    private const float RotScale = 2f;
    private const float WallJumpVertScale = 45f;
    private const float WallJumpHoriScale = 15f;

    private const float JumpScale = 40f;
    private const float maxVerticalSpeed = 0.2f;
    private float _gravityStrength = 80f;
    private float _maxFallingSpeed = 40f;

    private Vector3 _normalizedInputDirection;


    private const float _rayLength = 10f;
    
    
    float speedH = 2.0f;
    

    public Camera playerCamera;
    private float defaultFOV = 65f;
    private float sprintFOV = 80f;
    public float fovTransitionSpeed = 1f;

    public Vector3 gravityDirection = Vector3.down;

    private Vector3 _baseVelocity = Vector3.zero;

    GravityControl gravityControl;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        playerCamera = GetComponentInChildren<Camera>();
        playerCamera.fieldOfView = defaultFOV;
        gravityControl = GetComponent<GravityControl>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetIsGrounded(bool grounded)
    {
        _isGrounded = grounded;
    }

    public bool GetIsGrounded()
    {
        return _isGrounded;
    }

    public void SetIsOnWall(bool onWall, Vector3 closestPointOnWall)
    {
        _isOnWall = onWall;
        _vectorToWall = closestPointOnWall;
    }

    public void SetBaseVelocity(Vector3 baseVel)
    {
        _baseVelocity = baseVel;
        float currentRelativeSpeed = Vector3.Dot(_rigidbody.velocity, baseVel) / (baseVel.magnitude * baseVel.magnitude);
        Debug.Log(currentRelativeSpeed);
        if (currentRelativeSpeed != 1) 
        {
            _rigidbody.AddForce(((1 - (currentRelativeSpeed))/baseVel.magnitude)*baseVel, ForceMode.VelocityChange);
        }
    }

    public void RemoveBaseVelocity()
    {
        _baseVelocity = Vector3.zero;
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
        
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire2")) && _isRunWallJump)
        {
            _running = true;
        }
        else
        {
            _running = false;
        }
        
        float targetFOV = _running ? sprintFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovTransitionSpeed);
        
        if (Input.GetButton("Jump") && !_jumpDisabled && !_isHoggingJump){
            if (_isGrounded && !_userWallJumped)
            {
                _isHoggingJump = true;
                _userJumped = true;
                _isGrounded = false;
            }
            else if (_isOnWall && _isRunWallJump && !_userJumped) 
            {
                _isHoggingJump = true;
                _userWallJumped = true;
                _isOnWall = false;
            }
        }
        if (!Input.GetButton("Jump"))
        {
            _isHoggingJump = false;
        }

        if (_isGrav && (Input.GetKeyDown(KeyCode.Z) || Input.GetButtonUp("Fire4")))
        {
            gravityControl.RotateGravity(0);
        }
        else if (_isGrav && (Input.GetKeyDown(KeyCode.X) || Input.GetButtonUp("Fire5")))
        {
            gravityControl.RotateGravity(1);
        }

        // check for flying reaper switch
        if (_isFly)
        {
            SwitchToFlyingReaper();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.visible == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void FixedUpdate()
    {
        

        Quaternion userRot = Quaternion.AngleAxis(_yaw, transform.up);
        transform.rotation = userRot * transform.rotation;

        _yaw = 0.0f;


        Vector3 _normalizedInputDirection = Vector3.Normalize(transform.forward * _fbInput + transform.right * _lrInput); 
        ApplyFakeDrag(_normalizedInputDirection);
        ApplyFakeGrav();


        Vector3 _currentMovementWithoutVertical = GetHorizontalVel();
        float _currentMaxHorizontalSpeed = _running ? maxRunningSpeed : maxSpeed;
        if (_currentMaxHorizontalSpeed <= _currentMovementWithoutVertical.magnitude)
        {
            if (_rigidbody.velocity.x > 0)
            {
                _normalizedInputDirection.x = Mathf.Min(_normalizedInputDirection.x, 0);
            }
            if (_rigidbody.velocity.x < 0)
            {
                _normalizedInputDirection.x = Mathf.Max(_normalizedInputDirection.x, 0);
            }
            if (_rigidbody.velocity.y > 0)
            {
                _normalizedInputDirection.y = Mathf.Min(_normalizedInputDirection.y, 0);
            }
            if (_rigidbody.velocity.y < 0)
            {
                _normalizedInputDirection.y = Mathf.Max(_normalizedInputDirection.y, 0);
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

        Vector3 _lossFromRecentWallJump = _prevNormalizedWallJumpHori * Vector3.Dot(_normalizedInputDirection, _prevNormalizedWallJumpHori);
        Vector3 _finalDirection = _normalizedInputDirection - _lossFromRecentWallJump;

        if (!_running)
        {
            if (_isGrounded)
            {
                _rigidbody.AddForce(_finalDirection * MoveScale * _groundMultiplier, ForceMode.Force);
            }
            else
            {
                _rigidbody.AddForce(_finalDirection * MoveScale, ForceMode.Force);
            }
        }
        else {
            _rigidbody.AddForce(_finalDirection * RunScale, ForceMode.Force);
        }

        if (_userJumped)
        {
            Vector3 velocity = _rigidbody.velocity;
            float upVelocity = Vector3.Dot(velocity, -gravityDirection);
            

            _rigidbody.AddForce(transform.up * Math.Max((JumpScale-upVelocity), 0), ForceMode.VelocityChange);

            _userJumped = false;
            _isGrounded = false;
            _jumpDisabled = true;
            StartCoroutine(RegainJump());
        }

        if (_userWallJumped)
        {
            Debug.Log("wj");
            _userWallJumped = false;
            _isOnWall = false;

            Vector3 _vertToWall = gravityDirection * Vector3.Dot(_vectorToWall, gravityDirection);
            Vector3 _horiNormalToWall = Vector3.Normalize(_vectorToWall - _vertToWall);

            Vector3 velocity = _rigidbody.velocity;
            float upVelocity = Vector3.Dot(velocity, -gravityDirection);

            _prevNormalizedWallJumpHori = _horiNormalToWall;
            //Player will be unable to move in this ^ direction for a short period of time, see RegainFullHoriControl
            Vector3 finalHori = -1 * (_horiNormalToWall * WallJumpHoriScale);
            
            _rigidbody.AddForce(finalHori, ForceMode.VelocityChange);
            _rigidbody.AddForce(-GetHorizontalVel(), ForceMode.VelocityChange);

            _rigidbody.AddForce(-gravityDirection * Math.Max((WallJumpVertScale - upVelocity), 0), ForceMode.VelocityChange);
            _jumpDisabled = true;
            StartCoroutine(RegainJump());
            StartCoroutine(RegainFullHoriControl());
        }
    }

    void ApplyFakeDrag(Vector3 input)
    {
        if (_prevNormalizedWallJumpHori != Vector3.zero)
        {
            return;
        }
        Vector3 horizontal = GetHorizontalVel();
        Vector3 withoutInput = (horizontal - (input * Vector3.Dot(horizontal, input)));

        Vector3 drag = withoutInput * -1;
        if (_isGrounded)
        {
            _rigidbody.AddForce(drag * _groundDrag, ForceMode.VelocityChange);
        }
        else if (drag.magnitude > _minimumSpeedForAirDrag)
        {

            _rigidbody.AddForce(drag * _airDrag, ForceMode.VelocityChange);
            //Debug.Log("velocity: " + horizontal.ToString());
            //Debug.Log(drag);
        }

    }

    void ApplyFakeGrav()
    {
        if (Vector3.Dot(_rigidbody.velocity, gravityDirection) < _maxFallingSpeed)
        {
            _rigidbody.AddForce(gravityDirection * _gravityStrength, ForceMode.Acceleration);
        }
    }

    Vector3 GetHorizontalVel()
    {
        Vector3 velocity = _rigidbody.velocity - _baseVelocity;
        Vector3 vertical = gravityDirection * Vector3.Dot(velocity, gravityDirection);
        Vector3 horizontal = velocity - vertical;
        return horizontal;
    }
    
    private IEnumerator RegainJump()
    {
        yield return new WaitForSeconds(0.1f);
        _jumpDisabled = false;
    }

    private IEnumerator RegainFullHoriControl()
    {
        yield return new WaitForSeconds(0.4f);
        _prevNormalizedWallJumpHori = Vector3.zero;
    }

    void SwitchToFlyingReaper()
    {
        // get current position and rotation of the normal Reaper
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        GameObject flyingReaper = Instantiate(flyingReaperPrefab, currentPosition, currentRotation);

        // destroy the current Reaper object
        Destroy(gameObject);
    }
}
