using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _fbInput;
    private float _lrInput;
    private float _rotationInput;
    
    private bool _isGrounded;
    private bool _userJumped;
    
    private Rigidbody _rigidbody;

    private const float MoveScale = 0.2f;
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

        if (Input.GetButton("Jump") && _isGrounded)
        {
            _userJumped = true;
        }
    }

    private void FixedUpdate()
    {
        Vector3 userRot = transform.rotation.eulerAngles + new Vector3(0, _rotationInput * RotScale, 0);
        transform.rotation = Quaternion.Euler(userRot);
        
        _rigidbody.velocity += transform.forward * (_fbInput * MoveScale);
        _rigidbody.velocity += transform.right * (_lrInput * MoveScale);

        if (_userJumped)
        {
            _rigidbody.AddForce(Vector3.up * JumpScale, ForceMode.VelocityChange);
            _userJumped = false;
            _isGrounded = false;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        _isGrounded = true;
    }
    
    private void OnCollisionExit(Collision collision)
    {
        _isGrounded = false;
    }
}
