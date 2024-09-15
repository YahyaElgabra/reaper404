using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
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
        _textObject.text = ("Scrolls: " + scorer.score.ToString());
    }
}
