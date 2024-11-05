using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private float _udInput;
    // private float _ewInput;
    private float speedV = 1.0f;
    
    private float startY = 5;
    private float startZ = 10;
    private float minPitch = -45.0f;
    private float maxPitch = 89.99f;
    private float pitch = 0.0f;
    Vector3 startVector;
    
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
        startVector = new Vector3(0, 5, -10);
        maxPitch = Mathf.Atan2(startZ, startY) * Mathf.Rad2Deg - 0.01f;
        minPitch = maxPitch - 135;
        transform.localPosition = startVector;
        transform.LookAt(transform.parent);
    }
    void Update()
    {
        Vector2 _lookInput = _inputActions.Gameplay.Look.ReadValue<Vector2>();
        
        _udInput = _lookInput.y;
        // _ewInput = _lookInput.x;
        
        pitch += speedV * - _udInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, 0, 0);
        transform.localPosition = rotation * startVector;
        transform.LookAt(transform.parent);
        Vector3 adjustedRotation = transform.localEulerAngles;
        adjustedRotation.y = 0;
        adjustedRotation.z = 0;
        transform.localEulerAngles = adjustedRotation;
    }
}

