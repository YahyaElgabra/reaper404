using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpHitbox : MonoBehaviour
{
    public PlayerMovement player;
    private string _previousTouched;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.name != _previousTouched)
        {
            _previousTouched = trigger.name;
            player.SetIsGrounded(true);
            StartCoroutine(WipePrevious());
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        player.SetIsGrounded(false);
    }

    private IEnumerator WipePrevious()
    {
        yield return new WaitForSeconds(0.7f);
        _previousTouched = "";
    }
}
