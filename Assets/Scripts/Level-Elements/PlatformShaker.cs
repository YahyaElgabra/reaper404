using System.Collections;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public GameObject platformObject;
    public float shakeDuration = 1f;
    public float shakeIntensity = 0.1f;

    private Vector3 originalPosition;
    private bool isShaking = false;

    private void Start()
    {
        if (platformObject != null)
        {
            originalPosition = platformObject.transform.position;
        }
        else
        {
            Debug.LogWarning("Platform object not assigned!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isShaking && platformObject != null)
        {
            StartCoroutine(ShakeAndDestroyPlatform());
        }
    }

    private IEnumerator ShakeAndDestroyPlatform()
    {
        isShaking = true;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 shakeOffset = new Vector3(
                Random.Range(-shakeIntensity, shakeIntensity),
                0,
                Random.Range(-shakeIntensity, shakeIntensity)
            );

            platformObject.transform.position = originalPosition + shakeOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        platformObject.transform.position = originalPosition;

        Destroy(platformObject);
        Destroy(gameObject);
    }
}