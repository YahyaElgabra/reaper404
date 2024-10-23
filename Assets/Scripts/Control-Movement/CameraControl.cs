using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    float speedV = 2.0f;
    float minPitch = -45.0f;
    float maxPitch = 90.0f;
    private float pitch = 0.0f;
    void Update()
    {
        pitch += speedV * (Input.GetAxis("RJoy Y") - Input.GetAxis("Mouse Y"));
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.localEulerAngles = Vector3.right * pitch;
    }
}

