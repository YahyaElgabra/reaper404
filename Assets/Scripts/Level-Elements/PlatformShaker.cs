using System.Collections;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public GameObject platformObject;
    public float shakeDuration = 1f;
    public float timeLeft;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;
    private bool isShaking = false;

    private void Start()
    {
        timeLeft = shakeDuration;
        if (platformObject != null)
        {
            originalPosition = platformObject.transform.position;
        }
        else
        {
            Debug.LogWarning("platform object not assigned");
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

        while (timeLeft > 0)
        {
            Vector3 shakeOffset = new Vector3(
                Random.Range(-shakeMagnitude, shakeMagnitude),
                0,
                Random.Range(-shakeMagnitude, shakeMagnitude)
            );

            platformObject.transform.position = originalPosition + shakeOffset;

            timeLeft -= Time.deltaTime;
            yield return null;
        }

        platformObject.transform.position = originalPosition;

        Destroy(platformObject);
        Destroy(gameObject);
    }
}