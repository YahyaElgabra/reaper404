using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    bool phase = false;
    Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        // save Player's starting position
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
            if (phase)
            {
                collision.gameObject.transform.position = startingPosition;
            }
            else
            {
                collision.gameObject.transform.position = new Vector3(10, 0, 0);
            }
        }
    }
}
