using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    Vector3 startingPosition;
    public GameObject winScreen;
    // will assume that 0 means no special abilities, 1 means run+wall jump, 2 means tp, 3 means gravity, 4 means fly
    public int[] passes;
    int currentPass = 0;
    int currentAbility = -1;
    PlayerMovement movementScript;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        startingPosition = player.transform.position;
        movementScript = player.GetComponent<PlayerMovement>();
        ChangeAbility();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (currentPass == passes.Length - 1) {
                winScreen.SetActive(true);
            }
            currentPass++;
            ChangeAbility();
            collision.gameObject.transform.position = startingPosition;
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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
                break;
            case 3:
                movementScript._isGrav = true;
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
