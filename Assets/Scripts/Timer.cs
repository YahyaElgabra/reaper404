using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text _textObject;
    public float takenTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        _textObject = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        takenTime += Time.deltaTime;
        _textObject.text = ("Time taken: " + takenTime.ToString());
    }
}
