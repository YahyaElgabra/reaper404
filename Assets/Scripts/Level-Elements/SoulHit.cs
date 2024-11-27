using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHit : MonoBehaviour
{
    public ScoreTracker scorer;
    public float spinSpeed = 50f;
    public float bounceSpeed = 2f;
    public float bounceHeight = 0.5f;
    private float disappearDuration = 1.1f;

    private Vector3 startPosition;
    private bool isCollected = false;
    private Renderer soulRenderer;
    private Light soulLight;
    private AudioSource audioSource;

    void Start()
    {
        scorer = ScoreTracker.Instance;
        startPosition = transform.position;
        soulRenderer = GetComponent<Renderer>();
        soulLight = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isCollected)
        {
            // spin soul around Y axis
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

            // bounce soul up/down
            float newY = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight + startPosition.y;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Player" && !isCollected)
        {
            isCollected = true;
            
            GetComponent<Collider>().enabled = false;
            
            // sfx
            if (audioSource != null)
            {
                audioSource.Play();
            }
            
            if (scorer != null)
            {
                scorer.score += 1;
            }
            StartCoroutine(SpinAndDisappear());
        }
    }

    private IEnumerator SpinAndDisappear()
    {
        float startSpinSpeed = spinSpeed;
        float elapsedTime = 0f;
        float startLightIntensity = soulLight.intensity;

        Color originalColor = soulRenderer.material.color;

        while (elapsedTime < disappearDuration)
        {
            // increase spin speed
            spinSpeed = Mathf.Lerp(startSpinSpeed, startSpinSpeed * 48f, elapsedTime / disappearDuration);
            
            // reduce alpha
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / disappearDuration);
            soulRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            if (soulLight != null)
            {
                soulLight.intensity = Mathf.Lerp(startLightIntensity, 0f, elapsedTime / disappearDuration);
            }

            // rotate the object
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // make soul fully transparent
        soulRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        Destroy(this.gameObject);
    }
}