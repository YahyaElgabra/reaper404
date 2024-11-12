using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public float moveTarget;
    private float moveSpeed = 5f;
    public Material lineMaterial;
    private Rigidbody rb;
    private GameObject _spawnedPillar;
    private Vector3 _initialLocation;
    private bool _alreadyTriggered = false;
    public bool _moving = false;
    private LineRenderer lr;
    private Vector3 displacementFactor;

    // Start is called before the first frame update
    void Start()
    {
        _initialLocation = transform.position;
        rb = GetComponent<Rigidbody>();
        lr = gameObject.AddComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + transform.forward * moveTarget);
        displacementFactor = transform.forward * moveSpeed;


    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void FixedUpdate()
    {
        if (_moving)
        {
            rb.MovePosition(rb.position + (displacementFactor * Time.deltaTime));
            lr.SetPosition(0, transform.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_alreadyTriggered)
        {
            _alreadyTriggered = true;
            _moving = true;
            StartCoroutine(move());
        }
    }

    IEnumerator move()
    {
        

        while (((_initialLocation + transform.forward * moveTarget)-transform.position).sqrMagnitude > 0.1)
        {
            yield return null;
        }
        _moving = false;
    }
}