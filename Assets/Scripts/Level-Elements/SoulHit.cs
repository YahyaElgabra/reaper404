using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHit : MonoBehaviour
{
    public ScoreTracker scorer;

    void Start()
    {
        scorer = ScoreTracker.Instance;
    }
    
    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            scorer.score += 1;
        }
    }
}
