using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public GameObject hitboxObject; // Assign the hitbox GameObject in the Inspector
    public float shakeDuration = 1f; // Duration for shaking
    public float shakeIntensity = 0.1f; // Intensity of shake (how far it moves)

    private Vector3 originalPosition;
    private bool isShaking = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isShaking)
        {
            StartCoroutine(ShakeAndDisappear());
        }
    }

    private IEnumerator ShakeAndDisappear()
    {
        isShaking = true;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Apply a quick shake effect without modifying mesh or bounds
            Vector3 shakeOffset = new Vector3(
                Random.Range(-shakeIntensity, shakeIntensity),
                0,
                Random.Range(-shakeIntensity, shakeIntensity)
            );

            transform.position = originalPosition + shakeOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset position before destroying
        transform.position = originalPosition;

        // Destroy the hitbox object and then the platform itself
        if (hitboxObject != null)
        {
            Destroy(hitboxObject); // Delete the hitbox object
        }
        Destroy(gameObject); // Delete the platform itself
    }
}