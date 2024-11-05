using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    float speedV = 2.0f;
    float minPitch = -45.0f;
    float maxPitch = 89.99f;
    private float pitch = 25f;
    Vector3 startVector = new Vector3(0, 0, -12f);

    private void Start()
    {
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

