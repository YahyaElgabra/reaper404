using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    Vector3 startingPosition;
    public GameObject winScreen;
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
            PlayerMovement script = collision.gameObject.GetComponent<PlayerMovement>();

            if (script._isGrav)
            {
                winScreen.SetActive(true);
            }
            else if (script._isTP) {
            
                collision.gameObject.transform.position = startingPosition;
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                script._isTP = false;
                script._isGrav = true;
            }
            else if (script._isSecondRun)
            {

                collision.gameObject.transform.position = startingPosition;
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                script._isGrav = true;
                script._isSecondRun = false;
            }
            else
            {
                collision.gameObject.transform.position = startingPosition;
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                script._isSecondRun = true;

            }
        }
    }
}
