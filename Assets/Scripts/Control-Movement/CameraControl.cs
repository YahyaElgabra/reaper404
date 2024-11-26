using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private float _udInput;
    // private float _ewInput;
    private float speedV = 1.0f;
    
    float minPitch = -45.0f;
    float maxPitch = 89.99f;
    private float pitch = 25f;
    Vector3 startVector;
    float baseDistance = 5;
    float cameraDistance;

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
        cameraDistance = baseDistance + 15 * PlayerPrefs.GetFloat("cameraDistance", 0.5f);
        startVector = new Vector3(0, 0, -cameraDistance);
        transform.localPosition = startVector;
        transform.LookAt(transform.parent);
    }
    void Update()
    {
        Vector2 _lookInput = _inputActions.Gameplay.Look.ReadValue<Vector2>();
        
        _udInput = _lookInput.y;
        // _ewInput = _lookInput.x;
        
        pitch += speedV * - _udInput * Mathf.Lerp(0.25f, 1.5f, PlayerPrefs.GetFloat("cameraSensitivity", 0.5f));
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, 0, 0);
        transform.localPosition = rotation * startVector;
        transform.LookAt(transform.parent);
        Vector3 adjustedRotation = transform.localEulerAngles;
        adjustedRotation.y = 0;
        adjustedRotation.z = 0;
        transform.localEulerAngles = adjustedRotation;

        Transform player = transform.parent;
        Vector3 desiredCameraPosition = player.position - transform.forward * cameraDistance;
        RaycastHit hit;
        if (Physics.Raycast(player.position, desiredCameraPosition - player.position, out hit, cameraDistance, ~0) && !hit.collider.CompareTag("Soul"))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = desiredCameraPosition;
        }
    }
}

