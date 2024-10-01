using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disintegratingPlatform : MonoBehaviour
{
    private Vector3[] originalVertices;
    private Vector3[] modifiedVertices;
    private Mesh mesh;
    float shakeDuration = 1f;
    float shakeMagnitude = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = new Vector3[originalVertices.Length];
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

        float elapsedTime = 0f;

        Vector3 originalPosition = transform.position;
        
        while (shakeDuration > elapsedTime)
        {
            for (int i = 0; i < originalVertices.Length; i++)
            {
                Vector3 offset = Random.insideUnitSphere * shakeMagnitude;
                modifiedVertices[i] = originalVertices[i] + offset;
            }

            mesh.vertices = modifiedVertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
