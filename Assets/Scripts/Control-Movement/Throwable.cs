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
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    
        if (objectCollider != null)
        {
            objectCollider.enabled = true;
        }
    
        isThrown = true;
        
        StartCoroutine(SimulateThrowTrajectory(aimDirection, throwForce));
    }
    
    private IEnumerator SimulateThrowTrajectory(Vector3 direction, float force)
    {
        float simulationSpeed = 2.0f;
        float elapsedTime = 0f;
    
        Vector3 startPosition = transform.position;
        Vector3 velocity = direction * force;
    
        while (true)
        {
            elapsedTime += Time.deltaTime * simulationSpeed;
            
            Vector3 newPosition = startPosition + (velocity * elapsedTime) + (0.5f * Physics.gravity * elapsedTime * elapsedTime);
            
            if (Physics.Raycast(transform.position, newPosition - transform.position, out RaycastHit hit, (newPosition - transform.position).magnitude))
            {
                transform.position = hit.point;
    
                if (isThrown && hit.collider.CompareTag("death"))
                {
                    playerScript.OnThrowableHitDeath(gameObject);
                }
                else
                {
                    playerScript.TeleportPlayerAndDestroy(gameObject);
                }
                yield break;
            }
            
            transform.position = newPosition;
    
            yield return null;
        }
    }
    
    // OLD WAY
    // public void Throw(Vector3 aimDirection, float throwForce)
    // {
    //     if (rb != null)
    //     {
    //         rb.isKinematic = false;
    //         rb.useGravity = true;
    //         
    //         rb.AddForce(aimDirection * throwForce, ForceMode.Impulse);
    //     }
    //
    //     if (objectCollider != null)
    //     {
    //         objectCollider.enabled = true;
    //     }
    //
    //     isThrown = true;
    // }
    //
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (isThrown && collision.gameObject.CompareTag("death"))
    //     {
    //         playerScript.OnThrowableHitDeath(this.gameObject);
    //     }
    //     else if (isThrown && !collision.gameObject.CompareTag("Player"))
    //     {
    //         playerScript.TeleportPlayerAndDestroy(this.gameObject);
    //     }
    //     
    // }
}
