using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPicker : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public GameObject picker;
    int curr = 0;
    List<Button> buttons;
    int cols;
    picker pickerScript;
    bool inputAvailable = false;

    void Start()
    {
        buttons = new List<Button>();
        foreach (Transform child in gridLayoutGroup.transform)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                buttons.Add(button);
            }
        }
        cols = gridLayoutGroup.constraintCount;
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

        if (!up && !down && !right &&!left)
        {
            inputAvailable = true;
        }

        if (!inputAvailable)
        {
            return;
        }

        if (left)
        {
            curr = Mathf.Max(0, curr - 1);
            inputAvailable = false;
        }
        else if (right)
        {
            curr = Mathf.Min(buttons.Count - 1, curr + 1);
            inputAvailable = false;
        }
        else if (down)
        {
            curr = Mathf.Min(buttons.Count - 1, curr + cols);
            inputAvailable = false;
        }
        else if (up)
        {
            curr = Mathf.Max(0, curr - cols);
            inputAvailable = false;
        }

        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            buttons[curr].onClick.Invoke();
        }

        Vector3 buttonPosition = buttons[curr].transform.position;
        pickerScript.startingPosition = buttonPosition - new Vector3(150,0,0);
    }
}
