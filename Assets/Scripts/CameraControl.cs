using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float speedV = 2.0f;

    private float pitch = 0.0f;
    void Update()
    {
        pitch -= speedV * Input.GetAxis("Mouse Y");
        //transform.localEulerAngles = Vector3.right * pitch;
    }
}

