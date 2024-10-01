using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public Transform playerTransform;
    private bool isHeld = false;
    private Vector3 offset = new Vector3(1f, 2f, -1f);
    
    private Rigidbody rb;
    private Collider objectCollider;
    float timeToMove = 2f;
    
    public float throwForce = 10f;
    
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
    }
    
    void Update()
    {
        if (isHeld)
        {
            transform.position = playerTransform.position + playerTransform.rotation * offset;
            
            if ((Input.GetKey(KeyCode.C) || Input.GetButtonDown("Fire3")))
            {
                ThrowObject();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isHeld)
        {
            StartCoroutine(MoveToPlayer());
        }
    }

    private IEnumerator MoveToPlayer()
    {
        isHeld = true;
        
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
        
        Vector3 startPos = transform.position;

        for (float t = 0; t < 1; t += Time.deltaTime / timeToMove)
        {
            Vector3 targetPos = playerTransform.position + playerTransform.rotation * offset;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        
        transform.position = playerTransform.position + playerTransform.rotation * offset;
    }
    
    private void ThrowObject()
    {
        if (isHeld)
        {
            isHeld = false;

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                
                Vector3 throwDirection = playerTransform.forward;
                
                float angle = 45f;
                float throwHeight = Mathf.Tan(angle * Mathf.Deg2Rad);
                
                throwDirection.y += throwHeight;
                throwDirection.Normalize();
                
                rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            }
            
            if (objectCollider != null)
            {
                objectCollider.enabled = true;
            }
        }
    }
}
