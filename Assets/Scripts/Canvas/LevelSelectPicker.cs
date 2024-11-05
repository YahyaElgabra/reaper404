using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPicker : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    
    public GridLayoutGroup gridLayoutGroup;
    public GameObject picker;
    int curr = 0;
    List<Button> buttons;
    int cols;
    picker pickerScript;
    bool inputAvailable = false;

    void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        _inputActions.Gameplay.Enable();
    }

    void OnDisable()
    {
        _inputActions.Gameplay.Disable();
    }
    
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
        Vector2 _menuCursor = _inputActions.Gameplay.MenuCursor.ReadValue<Vector2>();
        
        float x = _menuCursor.x;
        float y = _menuCursor.y;

        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;

        if (x < -0.5f)
        {
            left = true;
        }
        else if (x > 0.5f)
        {
            right = true;
        }

        if (y < -0.5f)
        {
            down = true;
        }
        else if (y > 0.5f)
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

        if (_inputActions.Gameplay.Enter.IsPressed())
        {
            buttons[curr].onClick.Invoke();
        }

        Vector3 buttonPosition = buttons[curr].transform.position;
        pickerScript.startingPosition = buttonPosition - new Vector3(150,0,0);
    }
}
