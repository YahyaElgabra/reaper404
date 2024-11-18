using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class optionsManager : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    public GridLayoutGroup gridLayoutGroup;
    int curr = 0;
    public GameObject picker;
    picker pickerScript;
    List<GameObject> buttons;
    bool inputAvailable = false;
    public static bool sliderMode = false;
    Slider slider;
    Vector3 _button_offset = new Vector3(370, -10, 0);
    Vector3 _panel_offset = new Vector3(150, 0, 0);
    Vector3 _slider_offset = new Vector3(430, -10, 0);
    int _num_sliders = 3; // hardcoded to make life simpler
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
        buttons = new List<GameObject>();
        foreach (Transform child in gridLayoutGroup.transform)
        {
            if (child.gameObject.name == "Panel")
            {
                foreach (Transform panelChild in child)
                {
                    if (panelChild.gameObject.activeSelf == false)
                    {
                        continue;
                    }
                    buttons.Add(panelChild.gameObject);
                }
            }
            else
            {
                if (child.gameObject.activeSelf == false)
                {
                    continue;
                }
                buttons.Add(child.gameObject);
                if (child.gameObject.name == "master")
                {
                    child.gameObject.GetComponentInChildren<Slider>().value = PlayerPrefs.GetFloat("masterVolume", 1.0f);
                }
                if (child.gameObject.name == "distance")
                {
                    child.gameObject.GetComponentInChildren<Slider>().value = PlayerPrefs.GetFloat("cameraDistance", 0.5f);
                }
                if (child.gameObject.name == "sensitivity")
                {
                    child.gameObject.GetComponentInChildren<Slider>().value = PlayerPrefs.GetFloat("cameraSensitivity", 0.25f);
                }
            }
        }
        pickerScript = picker.GetComponent<picker>();
    }

    void Update()
    {
        Vector3 pickerPosition;
        if (gameObject.activeSelf)
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

            if (!up && !down && !right && !left && !Input.anyKey && !_inputActions.Gameplay.Escape.IsPressed())
            {
                inputAvailable = true;
            }

            if (!inputAvailable)
            {
                return;
            }

            if (left || up)
            {
                if (!sliderMode)
                {
                    curr = Mathf.Max(0, curr - 1);
                    inputAvailable = false;
                }    
            }
            else if (right || down)
            {
                if (!sliderMode)
                {
                    curr = Mathf.Min(buttons.Count - 1, curr + 1);
                    inputAvailable = false;
                }
            }

            if ((right || left) && sliderMode)
            {
                slider.value -= x * -0.0025f * Mathf.Lerp(0.25f, 3f, PlayerPrefs.GetFloat("cameraSensitivity", 0.25f));
                if (curr == 0) // master volume
                {
                    PlayerPrefs.SetFloat("masterVolume", slider.value);
                    AudioListener.volume = PlayerPrefs.GetFloat("masterVolume", 1.0f);
                }
                else if (curr == 1) // camera distance
                {
                    PlayerPrefs.SetFloat("cameraDistance", slider.value);
                }
                else if (curr == 2) // camera sensitivity
                {
                    PlayerPrefs.SetFloat("cameraSensitivity", slider.value);
                }
            }
            if (curr < _num_sliders)
            {
                pickerPosition = buttons[curr].transform.position - _button_offset;
            }
            else
            {
                pickerPosition = buttons[curr].transform.position - _panel_offset;
            }
            if ((_inputActions.Gameplay.Escape.IsPressed() || _inputActions.Gameplay.GravRight.IsPressed()) && inputAvailable)
            {
                inputAvailable = false;
                if (sliderMode)
                {
                    sliderMode = false;
                    pickerPosition = buttons[curr].transform.position - _button_offset;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            if (_inputActions.Gameplay.Enter.IsPressed())
            {
                inputAvailable = false;
                if (buttons[curr].name == "return")
                {
                    gameObject.SetActive(false);
                }
                else if (buttons[curr].name == "restart")
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else
                {
                    sliderMode = true;
                    slider = buttons[curr].GetComponentInChildren<Slider>();
                    pickerScript.startingPosition = pickerPosition + _slider_offset;
                }
            }
        }
        else
        {
            curr = 0;
            pickerPosition = buttons[curr].transform.position - _button_offset;
            sliderMode = false;
        }
        if (!sliderMode)
        {
            pickerScript.startingPosition = pickerPosition;
        }
    }
}
