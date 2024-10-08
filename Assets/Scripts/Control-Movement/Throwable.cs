using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Rigidbody rb;
    private Collider objectCollider;
    private bool isThrown = false;
    
    private Throwing playerScript;
    
    public void Initialize(Throwing playerScriptRef)
    {
        playerScript = playerScriptRef;
        
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
    }

    public void Throw(Vector3 aimDirection, float throwForce)
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            
            rb.AddForce(aimDirection * throwForce, ForceMode.Impulse);
        }

        if (objectCollider != null)
        {
            objectCollider.enabled = true;
        }

        isThrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrown && !collision.gameObject.CompareTag("Player"))
        {
            playerScript.TeleportPlayerAndDestroy(this.gameObject);
        }
    }
}
