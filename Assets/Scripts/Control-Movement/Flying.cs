using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flying : MonoBehaviour
{
    public float forwardSpeed = 10f; // forward speed
    public float movementSpeed = 5f; // up/down/left/right speed
    public float boostedSpeed = 20f; // boosting (shift) speed
    public float brakingSpeed = 5f; // braking (ctrl) speed
    public float accelerationRate = 5f; // boosting acceleration
    public float decelerationRate = 5f; // after boosting/braking deceleration
    private float _verticalInput;
    private float _horizontalInput;
    private Rigidbody _rigidbody;



    private float _currentSpeed; // to track the current forward speed
    private bool _isBoosting = false;
    private bool _isBraking = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // disable gravity for flying
        _rigidbody.useGravity = false; 

        // start at normal forward speed
        _currentSpeed = forwardSpeed;
    }

    private void Update()
    {
        // get input for vertical/horizontal movement
        _verticalInput = Input.GetAxisRaw("Vertical"); // left/right <-> A/D | left/right arrow keys | joystick left/right
        _horizontalInput = Input.GetAxisRaw("Horizontal"); // up/down <-> W/S | up/down arrow keys | joystick up/down

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
        if (!collision.gameObject.CompareTag("Goal"))
        {
            // reload the current scene ("death")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}