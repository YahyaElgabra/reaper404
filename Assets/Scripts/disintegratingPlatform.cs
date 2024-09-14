using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disintegratingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(disintegrate());
        }
    }

    IEnumerator disintegrate()
    {

        float shakeDuration = 1f;
        float elapsedTime = 0f;
        float shakeMagnitude = 0.1f;

        Vector3 originalPosition = transform.position;
        
        while (shakeDuration > elapsedTime)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.position = originalPosition + randomOffset;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
