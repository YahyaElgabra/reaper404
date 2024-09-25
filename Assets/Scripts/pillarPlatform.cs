using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarPlatform : MonoBehaviour
{
    private Vector3[] originalVertices;
    private Vector3[] modifiedVertices;
    private Mesh mesh;
    float shakeDuration = 1f;
    float shakeMagnitude = 0.1f;
    public GameObject spawnedPillar;

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
            GameObject spawned = Instantiate(spawnedPillar, transform.position - new Vector3(0, -4, 0), Quaternion.identity);
            spawned.transform.localScale += new Vector3(0, 10, 0);
        }
    }

    IEnumerator disintegrate()
    {

        float elapsedTime = 0f;

        Vector3 originalPosition = transform.position;

        while (shakeDuration > elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
