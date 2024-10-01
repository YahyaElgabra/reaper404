using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    PlayerMovement playerMovement;
    public int rotation = 4;
    bool _onCooldown = false;
    public float cooldownPeriod = 1f;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_onCooldown)
        {
            _onCooldown = true;
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            StartCoroutine(SwitchGravity());
        }
    }

    IEnumerator SwitchGravity()
    {
        // 0 changes "down" to (1, 0, 0), or "left" of our starting position,
        // 1 changes "down" to (-1, 0, 0), or "right" of our starting position,
        // 2 changes "down" to (0, 1, 0), or "up" of our starting position,
        // 3 changes "down" to (0, -1, 0), which is normal,
        // 4 changes "down" to (1, 0, 1), or "back" of our starting position,
        // 5 changes "down" to (0, 0, -1), or "forward" of our starting position,
        playerMovement.RotateGravity(rotation);
        yield return new WaitForSeconds(cooldownPeriod);
        _onCooldown = false;
    }
}
