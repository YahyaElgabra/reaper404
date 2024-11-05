using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuSelect : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public GameObject picker;
    int curr = 0;
    List<GameObject> buttons;
    picker pickerScript;
    bool inputAvailable = false;
    void Start()
    {
        buttons = new List<GameObject>();
        foreach (Transform child in gridLayoutGroup.transform)
        {
            buttons.Add(child.gameObject);
        }
        pickerScript = picker.GetComponent<picker>();
    }
    void Update()
    {
        float x = Input.GetAxis("DHor");
        float y = Input.GetAxis("DVer");

        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;

        if (x < -0.5f || Input.GetKeyDown(KeyCode.A))
        {
            left = true;
        }
        else if (x > 0.5f || Input.GetKeyDown(KeyCode.D))
        {
            right = true;
        }

        if (y < -0.5f || Input.GetKeyDown(KeyCode.S))
        {
            down = true;
        }
        else if (y > 0.5f || Input.GetKeyDown(KeyCode.W))
        {
            up = true;
        }

        if (!up && !down && !right && !left)
        {
            inputAvailable = true;
        }

        if (!inputAvailable)
        {
            return;
        }

        if (left || up)
        {
            curr = Mathf.Max(0, curr - 1);
            inputAvailable = false;
        }
        else if (right || down)
        {
            curr = Mathf.Min(buttons.Count - 1, curr + 1);
            inputAvailable = false;
        }

        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            if (buttons[curr].name == "start")
            {
                SceneManager.LoadScene("LevelSelect");
            }
            else if (buttons[curr].name == "exit")
            {
                Application.Quit();
            }
        }

        Vector3 buttonPosition = buttons[curr].transform.position;
        pickerScript.startingPosition = buttonPosition - new Vector3(150, 0, 0);
    }
}
