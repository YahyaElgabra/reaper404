using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flying : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    
    public float forwardSpeed = 10f; // forward speed
    private float movementSpeed = 10f; // up/down/left/right speed
    private float boostedSpeed = 10f; // boosting (shift) speed
    private float brakingSpeed = 2f; // braking (ctrl) speed
    private float accelerationRate = 5f; // boosting acceleration
    private float decelerationRate = 5f; // after boosting/braking deceleration
    private float _verticalInput;
    private float _horizontalInput;
    private Rigidbody _rigidbody;



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
        _isBoosting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        _isBraking = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // adjust the forward speed based on input
        AdjustSpeed();
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

    private void FixedUpdate()
    {
        if(!_canMove) return;

        // apply constant forward movement
        Vector3 forwardMovement = transform.forward * (_currentSpeed * Time.fixedDeltaTime);
        
        // apply controlled vertical/horizontal movement (up/down, right/left)
        Vector3 controlledMovement = (transform.up * _verticalInput + transform.right * _horizontalInput) * (movementSpeed * Time.fixedDeltaTime);
        
        // move player (combine forward and controlled movement)
        _rigidbody.MovePosition(_rigidbody.position + forwardMovement + controlledMovement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check if the player colided with anything that is not the goal
        if (!collision.gameObject.CompareTag("Goal"))
        {
            // reload the current scene ("death")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}