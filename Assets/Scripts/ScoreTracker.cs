using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public int score;
    public float time;
    public bool stopTime;
    private static ScoreTracker _instance;
    public static ScoreTracker Instance
    {
        get
        {
            return _instance;
        }
        private set { }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this) {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTime)
        {
            time += Time.deltaTime;
        }
    }
}
