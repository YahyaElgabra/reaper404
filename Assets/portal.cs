using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    Vector3 startingPosition;
    bool phase = false;
    // Start is called before the first frame update
    void Start()
    {
        // save Player's starting position
        GameObject player = GameObject.FindWithTag("reaper");
        startingPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("reaper"))
        {
            if (!phase)
            {
                collision.gameObject.transform.position = startingPosition;
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                if (rb != null) {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
            else
            {
                collision.gameObject.transform.position = new Vector3(10, 0, 0);
            }
        }
    }
}
