using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirDisintegratingPlatform : MonoBehaviour
{
    public GameObject platformObject; // assign platform (to shake)
    public GameObject triggerObject; // assign trigger
    public float shakeDuration = 1f;
    public float shakeMagnitude = 0.1f;
    public bool steppedOn = false;

    private Vector3 originalPosition;
    private bool isShaking = false;

    void Start()
    {
        if (platformObject != null)
        {
            originalPosition = platformObject.transform.position;
        }
        else
        {
            Debug.LogWarning("Platform object not assigned.");
        }
    }

    void Update()
    {
        if (steppedOn && !isShaking)
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
                Random.Range(-shakeMagnitude, shakeMagnitude),
                0,
                Random.Range(-shakeMagnitude, shakeMagnitude)
            );

            platformObject.transform.position = originalPosition + shakeOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        platformObject.transform.position = originalPosition;
        Destroy(triggerObject); 
        Destroy(platformObject);
        Destroy(gameObject);
    }
}