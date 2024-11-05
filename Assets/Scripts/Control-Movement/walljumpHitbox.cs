using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walljumpHitbox : MonoBehaviour
{
    public PlayerMovement player;
    private Vector3 _closest;
    private string _previousTouched;
    private Collider touching;
    private bool _isTouching = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.CompareTag("Soul"))
        {
            return;
        }
        
        if (trigger.name != _previousTouched) {
            _previousTouched = trigger.name;
            _closest = trigger.ClosestPoint(transform.position);
            player.SetIsOnWall(true, _closest - transform.position);
            StartCoroutine(WipePrevious());
            _isTouching = true;
            touching = trigger;
        }
        Debug.Log(trigger.name);
    }

    void FixedUpdate()
    {
        if (_isTouching && (touching == null))
        {
            player.SetIsOnWall(false, _closest - transform.position);
            _isTouching = false;
            Debug.Log("bro died");
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        if (!trigger.CompareTag("Soul"))
        {
            player.SetIsOnWall(false, _closest - transform.position);
        }
        // Debug.Log("left");
    }

    private IEnumerator WipePrevious()
    {
        yield return new WaitForSeconds(0.7f);
        _previousTouched = "";
    }
}
