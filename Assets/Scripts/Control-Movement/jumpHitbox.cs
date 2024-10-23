using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpHitbox : MonoBehaviour
{
    public PlayerMovement player;
    private string _previousTouched;
    private HashSet<string> _latest;
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
            Debug.Log("Ground");
            
            _latest.Add(trigger.name);
            _previousTouched = "";
            //StartCoroutine(WipePrevious());
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
            Debug.Log("Air");
        }
    }

    private IEnumerator WipePrevious()
    {
        yield return new WaitForSeconds(0.7f);
        _previousTouched = "";
    }
}
