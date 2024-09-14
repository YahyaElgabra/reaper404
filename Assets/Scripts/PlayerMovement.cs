using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool _isSecondRun = true;

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

    private const float MoveScale = 0.2f;
    private const float RunScale = 0.5f;
    private const float RotScale = 2f;
    private const float JumpScale = 5f;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        _fbInput = Input.GetAxis("Vertical");
        _lrInput = Input.GetAxis("Horizontal");
        
        if (Input.GetKey(KeyCode.Z))
        {
            _rotationInput = -1;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            _rotationInput = 1;
        }
        else
        {
            _rotationInput = 0;
        }
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

        if (!_running)
        {
            _rigidbody.velocity += transform.forward * (_fbInput * MoveScale);
            _rigidbody.velocity += transform.right * (_lrInput * MoveScale);
        }
        else {
            _rigidbody.velocity += transform.forward * (_fbInput * RunScale);
            _rigidbody.velocity += transform.right * (_lrInput * RunScale);
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
        Debug.Log("grounded");
        Debug.Log(collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider trigger)
    {
        _isOnWall = true;
        Debug.Log("trigger");
        Debug.Log(trigger.gameObject.name);
    }

    private void OnTriggerExit(Collider trigger)
    {
        _isOnWall = false;
        Debug.Log("trigger out");
        Debug.Log(trigger.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
    }
}
