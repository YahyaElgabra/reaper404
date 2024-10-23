using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pillarPlatform : MonoBehaviour
{
    public GameObject pillarPrefab;
    private GameObject _spawnedPillar;
    private bool _alreadyTriggered = false;
    private Vector3 _absoluteDir;
    private Rigidbody rb;
    public float growthTarget = 3f;
    private bool _growing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (_growing)
        {
            Vector3 growthFactor = Vector3.up * 0.01f;
            _spawnedPillar.transform.localScale += growthFactor;
            Vector3 newPosition = rb.position - transform.TransformDirection(growthFactor * 2.5f);
            rb.MovePosition(newPosition);
            
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_alreadyTriggered)
        {
            _spawnedPillar = Instantiate(pillarPrefab, transform.position - transform.up*2.5f, transform.rotation, this.transform);
            _spawnedPillar.transform.localScale = new Vector3(1, 1, 1) - Vector3.up * 0.99f;
            rb = _spawnedPillar.GetComponent<Rigidbody>();
            _alreadyTriggered = true;
            _growing = true;
            StartCoroutine(expand());
        }
    }

    IEnumerator expand()
    {
        while (Vector3.Project(_spawnedPillar.transform.localScale, Vector3.up).magnitude < growthTarget)
        {
            yield return null;
        }
        _growing = false;
    }
}
