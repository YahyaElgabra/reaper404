using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathFloor : MonoBehaviour
{
    Vector3 startingPosition;
    private PlayerInputActions _inputActions;
    // Start is called before the first frame update
    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        startingPosition = player.transform.position;
        
        // _startingRotation = Quaternion.Euler(0, 80, 0);
        // player.transform.rotation = _startingRotation;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject goal = GameObject.FindWithTag("Finish");
            goal.GetComponent<portal>().ResetPass();
        }
    }
}
