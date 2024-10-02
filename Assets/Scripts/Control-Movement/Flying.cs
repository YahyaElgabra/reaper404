using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flying : MonoBehaviour
{
    public float forwardSpeed = 10f; // forward speed
    public float movementSpeed = 5f; // up/down/left/right speed
    private float _verticalInput;
    private float _horizontalInput;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // disable gravity for flying
        _rigidbody.useGravity = false; 
    }

    private void Update()
    {
        // get input for vertical/horizontal movement
        _verticalInput = Input.GetAxisRaw("Vertical"); // left/right <-> A/D | left/right arrow keys | joystick left/right
        _horizontalInput = Input.GetAxisRaw("Horizontal"); // up/down <-> W/S | up/down arrow keys | joystick up/down
    }

    private void FixedUpdate()
    {
        // apply constant forward movement
        Vector3 forwardMovement = transform.forward * forwardSpeed * Time.fixedDeltaTime;
        
        // apply controlled vertical/horizontal movement (up/down, right/left)
        Vector3 controlledMovement = (transform.up * _verticalInput + transform.right * _horizontalInput) * movementSpeed * Time.fixedDeltaTime;
        
        // move player (combine forward and controlled movement)
        _rigidbody.MovePosition(_rigidbody.position + forwardMovement + controlledMovement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player collides with any platform
        if (!collision.gameObject.CompareTag("Goal"))
        {
            // Reload the current scene, simulating player death
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}