using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpHitbox : MonoBehaviour
{
    public PlayerMovement player;
    private string _previousTouched;
    private HashSet<string> _latest;
    private Vector3 baseVel;
    // Start is called before the first frame update
    void Start()
    {
        _latest = new HashSet<string>();
        player = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.name != _previousTouched)
        {
            _previousTouched = trigger.name;
            player.SetIsGrounded(true);
            //Debug.Log("Ground");
            
            _latest.Add(trigger.name);
            _previousTouched = "";
            //StartCoroutine(WipePrevious());s
        }
    }

    private void OnTriggerStay(Collider trigger)
    {
        if (trigger.tag == "MovingPlatform" && trigger.GetComponent<Rigidbody>().velocity != baseVel)
        {
            baseVel = trigger.GetComponent<Rigidbody>().velocity;
            player.SetBaseVelocity(trigger.GetComponent<Rigidbody>().velocity);
            //Debug.Log(trigger.GetComponent<Rigidbody>().velocity);
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        if (_latest.Contains(trigger.name))
        {
            _latest.Remove(trigger.name);
        }
        if (_latest.Count == 0) { 
            player.SetIsGrounded(false);
            //Debug.Log("Air");
        }
        if (trigger.tag == "MovingPlatform")
        {
            baseVel = Vector3.zero;
            player.RemoveBaseVelocity();
        }
    }

    private IEnumerator WipePrevious()
    {
        yield return new WaitForSeconds(0.7f);
        _previousTouched = "";
    }
}
