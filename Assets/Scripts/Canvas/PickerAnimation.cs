using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class picker : MonoBehaviour
{
    public Vector3 startingPosition;
    float moveDistance = 5f;
    float moveSpeed = 6.0f;
    void Start()
    {
        startingPosition = transform.position;
    }
    void Update()
    {
        float newPosX = startingPosition.x + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = new Vector3(newPosX, startingPosition.y, 0);
    }
}
