using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    private Text _textObject;
    public ScoreTracker scorer;

    void Start()
    {
        scorer = ScoreTracker.Instance;
        _textObject = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // _textObject.text = ("Time taken: " + scorer.time.ToString());
        float roundedTime = Mathf.Round(scorer.time * 10f) / 10f;
        _textObject.text = ("Time taken: " + roundedTime.ToString());
    }
}
