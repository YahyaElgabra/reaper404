using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flying : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    
    private float forwardSpeed = 8f; // forward speed
    private float movementSpeed = 14f; // up/down/left/right speed
    private float boostedSpeed = 20f; // boosting (shift) speed
    private float brakingSpeed = 5f; // braking (ctrl) speed
    private float accelerationRate = 5f; // boosting acceleration
    private float decelerationRate = 5f; // after boosting/braking deceleration
    private float tiltAmount = 15f; // tilt angle when moving in any direction
    private float tiltSpeed = 5f; // speed of tilting to and from center

    private float _verticalInput;
    private float _horizontalInput;
    private Rigidbody _rigidbody;
    private Quaternion _originalRotation;

    private float _currentSpeed; // to track the current forward speed
    private bool _isBoosting = false;
    private bool _isBraking = false;
    private bool _canMove = false; // for movement delay at start

    void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        _inputActions.Gameplay.Enable();
    }

    void OnDisable()
    {
        _inputActions.Gameplay.Disable();
    }
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // disable gravity for flying
        _rigidbody.useGravity = false; 

        // store the initial rotation as the default (centered) rotation
        _originalRotation = transform.rotation;

        // start at normal forward speed
        _currentSpeed = forwardSpeed;

        // delay movement
        StartCoroutine(DelayMovement());        
    }

    private IEnumerator DelayMovement()
    {
        
        yield return new WaitForSeconds(2.5f);
        _canMove = true;
    }

    private void Update()
    {
        if(!_canMove) return;

        // get input for vertical/horizontal movement
        Vector2 _moveInput = _inputActions.Gameplay.Move.ReadValue<Vector2>();
        
        _verticalInput = _moveInput.y; // left/right <-> A/D | left/right arrow keys | joystick left/right
        _horizontalInput = _moveInput.x; // up/down <-> W/S | up/down arrow keys | joystick up/down
        // check for boosting (shift) and braking (ctrl)
        _isBoosting = _inputActions.Gameplay.Run.IsPressed();
        _isBraking = _inputActions.Gameplay.ThrowHold.IsPressed() || Input.GetKey(KeyCode.LeftControl);

        // adjust the forward speed based on input
        AdjustSpeed();

        // apply tilt based on movement direction
        ApplyTilt();
    }

    private void AdjustSpeed()
    {
        if (_isBoosting)
        {
            // increase forward speed toward boosted speed
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, boostedSpeed, accelerationRate * Time.deltaTime);
        }
        else if (_isBraking)
        {
            // decrease forward speed toward braking speed
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, brakingSpeed, decelerationRate * Time.deltaTime);
        }
        else
        {
            // gradually return to normal speed when not boosting/braking
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, forwardSpeed, decelerationRate * Time.deltaTime);
        }
    }

    private void ApplyTilt()
    {
        // determine tilt based on input
        float tiltX = -_verticalInput * tiltAmount; // tilt up/down
        float tiltZ = -_horizontalInput * tiltAmount; // tilt left/right

        // calculate target rotation for tilting
        Quaternion targetRotation = _originalRotation * Quaternion.Euler(tiltX, 0, tiltZ);

        // smoothly rotate towards target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if(!_canMove) return;

        // apply constant forward movement
        Vector3 forwardMovement = transform.forward * _currentSpeed * Time.fixedDeltaTime;
        
        // apply controlled vertical/horizontal movement (up/down, right/left)
        Vector3 controlledMovement = (transform.up * _verticalInput + transform.right * _horizontalInput) * movementSpeed * Time.fixedDeltaTime;
        
        // move player (combine forward and controlled movement)
        _rigidbody.MovePosition(_rigidbody.position + forwardMovement + controlledMovement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check if the player colided with anything that is not the goal
        if (!collision.gameObject.CompareTag("Finish"))
        {
            // reload the current scene ("death")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}