using System.Collections;
using UnityEngine;

public class PillarStacker : MonoBehaviour
{
    public GameObject pillarPrefab; 
    public int pillarsToSpawn = 3;
    public float gapBetweenPillars = 0.1f;
    public float spawnSpeed = 10f; 
    public Material lineMaterial; // Assignable material for the line

    private bool isTriggered = false;
    private float pillarHeight;
    private int pillarCounter = 1; // track spawned pillar names
    private LineRenderer lineRenderer;

    public AudioSource audioSource;

    void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf != null)
        {
            Vector3 meshSize = mf.mesh.bounds.size;
            Vector3 localScale = transform.localScale;
            pillarHeight = meshSize.y * localScale.y;
        }
        else
        {
            Debug.LogWarning("meshfilter not found on pillar.");
            pillarHeight = 1f;
        }

        // line renderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        lineRenderer.positionCount = 2; // start and end points
        lineRenderer.useWorldSpace = true;

        SetLineToFinalPosition();
    }

    private void SetLineToFinalPosition()
    {
    Vector3 start = transform.position;
    Vector3 localDown = -transform.up;

    // calculate final position to the middle of the last pillar
    Vector3 offset = localDown * (pillarHeight + gapBetweenPillars) * pillarsToSpawn;
    Vector3 end = start + offset + (localDown * pillarHeight / 2);

    lineRenderer.SetPosition(0, start);
    lineRenderer.SetPosition(1, end);
    }

    private void OnTriggerEnter(Collider other)
    {
    if (!isTriggered && (other.gameObject.name == "walljumpHitbox" || other.gameObject.name == "jumpHitbox"))
    {
        isTriggered = true;
        StartCoroutine(SpawnPillars());
    }
}

    private IEnumerator SpawnPillars()
    {
        audioSource.Play();
        Vector3 spawnPosition = transform.position;

        for (int i = 1; i <= pillarsToSpawn; i++)
        {
            // calculate local downward direction for the pillar
            Vector3 localDown = -transform.up;

            // offset the target position for each new pillar
            Vector3 offset = localDown * (pillarHeight + gapBetweenPillars);
            Vector3 targetPosition = spawnPosition + offset;

            // spawn, scale, and rename the new pillar
            GameObject newPillar = Instantiate(pillarPrefab, spawnPosition, transform.rotation);
            newPillar.name = "Expanding Pillar (Clone) " + pillarCounter++; // assign unique name
            ScalePillarToMatchOriginal(newPillar);
            yield return StartCoroutine(MovePillarIntoPosition(newPillar, targetPosition));

            // update next pillar spawn position to the last pillar's final position
            spawnPosition = targetPosition;
        }
    }

    private void ScalePillarToMatchOriginal(GameObject pillar)
    {
        // scale the new pillar to match the original pillar
        MeshFilter originalMeshFilter = GetComponent<MeshFilter>();
        MeshFilter newMeshFilter = pillar.GetComponent<MeshFilter>();

        if (originalMeshFilter != null && newMeshFilter != null)
        {
            Vector3 originalSize = originalMeshFilter.mesh.bounds.size;
            Vector3 originalScale = transform.localScale;
            Vector3 originalWorldSize = new Vector3(
                originalSize.x * originalScale.x,
                originalSize.y * originalScale.y,
                originalSize.z * originalScale.z
            );

            Vector3 newSize = newMeshFilter.mesh.bounds.size;
            Vector3 newScale = new Vector3(
                originalWorldSize.x / newSize.x,
                originalWorldSize.y / newSize.y,
                originalWorldSize.z / newSize.z
            );

            pillar.transform.localScale = newScale;
        }
    }

    private IEnumerator MovePillarIntoPosition(GameObject pillar, Vector3 targetPosition)
    {
        while (Vector3.Distance(pillar.transform.position, targetPosition) > 0.01f)
        {
            pillar.transform.position = Vector3.MoveTowards(pillar.transform.position, targetPosition, spawnSpeed * Time.deltaTime);
            yield return null;
        }

        // ensure alignment
        pillar.transform.position = targetPosition;
    }
}