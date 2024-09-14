using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathFloor : MonoBehaviour
{
    Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        startingPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = startingPosition;
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
