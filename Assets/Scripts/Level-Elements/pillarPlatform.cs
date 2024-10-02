using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarPlatform : MonoBehaviour
{
    public GameObject pillarPrefab;
    private GameObject _spawnedPillar;
    private bool _alreadyTriggered = false;

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
        if (collision.gameObject.CompareTag("Player") && !_alreadyTriggered)
        {
            _spawnedPillar = Instantiate(pillarPrefab, transform.position - new Vector3(0f, 5f, 0f), Quaternion.identity, this.transform);
            _alreadyTriggered = true;
            StartCoroutine(expand());
        }
    }

    IEnumerator expand()
    {
        Vector3 growthFactor = new Vector3(0f, 2.8f, 0f);
        float growthTarget = 3f;

        while (_spawnedPillar.transform.localScale.y < growthTarget)
        {
            _spawnedPillar.transform.localScale += growthFactor * Time.deltaTime;
            _spawnedPillar.transform.position -= (growthFactor*5f/2f) * Time.deltaTime;
            yield return null;
        }
    }
}
