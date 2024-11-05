using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointRingMovement : MonoBehaviour
{
    public float amplitude = -0.5f;  
    public float speed = 1f;      
    public float delay = 0f; // modify in each ring (diff values) to create pulsating effect
    // public float emissionIntensity = 20f; 

    private Vector3 localStartPosition; 
    // private Material ringMaterial; 

    void Start()
    {
        localStartPosition = transform.localPosition;
        // ringMaterial = GetComponent<Renderer>().material;
        // ringMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // update Y (sin wave and delay)
        float newY = Mathf.Sin(Time.time * speed + delay) * amplitude;
        transform.localPosition = new Vector3(localStartPosition.x, localStartPosition.y + newY, localStartPosition.z);

        // adjust emission based on Y
        // float emissionValue = Mathf.Abs(Mathf.Sin(Time.time * speed + delay)) * emissionIntensity;
        // Color baseEmissionColor = Color.white;
        // ringMaterial.SetColor("_EmissionColor", baseEmissionColor * emissionValue);
    }
}