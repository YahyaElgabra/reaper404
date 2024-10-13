using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float speedV = 2.0f;
    public float minPitch = -45.0f;
    public float maxPitch = 45.0f;
    private float pitch = 0.0f;
    void Update()
    {
        pitch += speedV * (Input.GetAxis("RJoy Y") - Input.GetAxis("Mouse Y"));
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.localEulerAngles = Vector3.right * pitch;
    }
}

