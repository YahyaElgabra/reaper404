using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpHitbox : MonoBehaviour
{
    public PlayerMovement player;
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
        player.SetIsGrounded(true);
        //Debug.Log("Ground at " + trigger.name);

        
        if (trigger.tag == "DisappearingPlatform")
        {
            float time = trigger.GetComponent<PlatformTrigger>().timeLeft;
            string name = trigger.transform.parent.name;
            StartCoroutine(RemoveDisappeared(name, time));
            _latest.Add(name);
        }
        else if (trigger.tag == "MovingPlatform")
        {
            string name = trigger.transform.parent.name;
            _latest.Add(name);
        }
        else
        {
            _latest.Add(trigger.name);
        }
    }

    private void OnTriggerStay(Collider trigger)
    {
        if (trigger.tag == "MovingPlatform" && trigger.GetComponent<Rigidbody>().velocity != baseVel)
        {
            if (trigger.GetComponent<movingPlatform>()._moving == true) {
                baseVel = trigger.GetComponent<Rigidbody>().velocity;
                player.SetBaseVelocity(trigger.GetComponent<Rigidbody>().velocity);
                //Debug.Log(trigger.GetComponent<Rigidbody>().velocity);
            }
            else
            {
                player.RemoveBaseVelocity();
            }
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        string name = trigger.name;
        if (trigger.tag == "MovingPlatform" | trigger.tag == "DisappearingPlatform")
        {
            name = trigger.transform.parent.name;
        }
        if (_latest.Contains(name))
        {
            _latest.Remove(name);
            //Debug.Log("Lost a real one: " + trigger.name);
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

    IEnumerator RemoveDisappeared(string platformToDelete, float time)
    {
        float elapsedTime = 0f;

        while (time > elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (_latest.Contains(platformToDelete))
        {
            _latest.Remove(platformToDelete);
            //Debug.Log("Deleted a real one: " + platformToDelete);
        }
        if (_latest.Count == 0)
        {
            player.SetIsGrounded(false);
            //Debug.Log("Air");
        }
    }
}
