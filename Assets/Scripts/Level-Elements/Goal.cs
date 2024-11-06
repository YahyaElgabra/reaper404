using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    Vector3 startingPosition;
    GameObject winScreen;
    // will assume that 0 means no special abilities, 1 means run+wall jump, 2 means tp, 3 means gravity, 4 means fly
    public int[] passes;
    int currentPass = 0;
    int currentAbility = -1;
    public int[] charges;
    int chargeIndex = 0;
    PlayerMovement movementScript;
    Throwing throwingScript;
    GravityControl gravityScript;
    AbilitiesUI abilitiesUI = null;
    Rigidbody rb;

    private float latestCollisionTime = 0f;

    Quaternion startingRotation;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        startingPosition = player.transform.position;
        startingRotation = player.transform.rotation;
        movementScript = player.GetComponent<PlayerMovement>();
        throwingScript = player.GetComponent<Throwing>();
        gravityScript = player.GetComponent<GravityControl>();
        rb = player.GetComponent<Rigidbody>();
        GameObject abilitiesObject = GameObject.FindGameObjectWithTag("AbilitiesUI");
        if (abilitiesObject != null)
        {
            abilitiesUI = abilitiesObject.GetComponent<AbilitiesUI>();
            if (charges.Length > 0)
            {
                abilitiesUI.updateCharges(charges[chargeIndex]);
            }
        }
        GameObject canvas = GameObject.FindWithTag("Canvas");
        foreach (Transform child in canvas.transform)
        {
            if (child.CompareTag("WinScreen"))
            {
                winScreen = child.gameObject;
            }
        }
         ChangeAbility();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // if (throwingScript != null && throwingScript.justTeleported)
            // {
            //     // If the player just teleported, skip this collision to avoid unexpected pass increment
            //     return;
            // }
            
            if (currentPass == passes.Length - 1) {
                winScreen.SetActive(true);
                rb.isKinematic = true;
                ScoreTracker.stopTime = true;
            }
            else
            {
                if (Time.time - latestCollisionTime < 0.5f)
                {
                    return;
                }
                latestCollisionTime = Time.time;
                currentPass++;
                collision.gameObject.transform.position = startingPosition;
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                movementScript.gravityDirection = Vector3.down;
                collision.gameObject.transform.rotation = startingRotation;
                ChangeAbility();
                if (abilitiesUI != null)
                {
                    abilitiesUI.updateIcons(currentPass);
                    if (charges.Length > 0)
                    {
                        abilitiesUI.updateCharges(charges[chargeIndex]);
                    }
                }
            }
        }
    }

    void ChangeAbility()
    {
        // first, disable previous ability
        switch (currentAbility)
        {
            case -1:
            case 0:
                break;
            case 1:
                movementScript._isRunWallJump = false;
                break;
            case 2:
                movementScript._isTP = false;
                break;
            case 3:
                movementScript._isGrav = false;
                break;
            case 4:
                movementScript._isFly = false;
                break;
            default:
                Debug.Log("invalid passes array");
                break;
        }

        // next, enable new ability
        if (currentPass == passes.Length)
        {
            return;
        }
        currentAbility = passes[currentPass];
        switch (currentAbility)
        {
            case 0:
                break;
            case 1:
                movementScript._isRunWallJump = true;
                break;
            case 2:
                movementScript._isTP = true;
                if (chargeIndex < charges.Length)
                {
                    throwingScript.charges = charges[chargeIndex];
                }
                if (chargeIndex < charges.Length - 1)
                {
                    chargeIndex++;
                }
                break;
            case 3:
                movementScript._isGrav = true;
                if (chargeIndex < charges.Length)
                {
                    gravityScript.charges = charges[chargeIndex];
                }
                if (chargeIndex < charges.Length - 1)
                {
                    chargeIndex++;
                }
                break;
            case 4:
                movementScript._isFly = true;
                break;
            default:
                Debug.Log("invalid passes array");
                break;
        }
    }
}
