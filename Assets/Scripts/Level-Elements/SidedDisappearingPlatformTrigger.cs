using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disintegHitbox : MonoBehaviour
{
    public dirDisintegratingPlatform platform;
    private bool _stillTouching;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.name == "jumpHitbox")
        {
            _stillTouching = true;
            StartCoroutine(Wait());
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        if (trigger.name == "jumpHitbox")
        {
            _stillTouching = false;
            
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        if (_stillTouching)
        {
            platform.steppedOn = true;
        }
        
    }
}