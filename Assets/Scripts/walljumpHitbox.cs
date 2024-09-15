using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walljumpHitbox : MonoBehaviour
{
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider trigger)
    {
        player.SetIsOnWall(true);
    }

    private void OnTriggerExit(Collider trigger)
    {
        player.SetIsOnWall(false);
    }
}
