using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    float startY = 5;
    float startZ = 10;
    float speedV = 2.0f;
    float minPitch = -45.0f;
    float maxPitch = 89.99f;
    private float pitch = 0.0f;
    Vector3 startVector;

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
        pitch += speedV * (Input.GetAxis("RJoy Y") - Input.GetAxis("Mouse Y"));
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

