using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuSelect : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    
    public GridLayoutGroup gridLayoutGroup;
    public GameObject picker;
    int curr = 0;
    List<GameObject> buttons;
    picker pickerScript;
    bool inputAvailable = false;
    public GameObject optionsMenu;
    public AudioSource audioSource;

    public AudioSource audioSourceCHOOSE;
    
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
            buttons.Add(child.gameObject);
        }
        pickerScript = picker.GetComponent<picker>();
    }
    void Update()
    {
        Vector2 _menuCursor = _inputActions.Gameplay.MenuCursor.ReadValue<Vector2>();
        
        float x = _menuCursor.x;
        float y = _menuCursor.y;
        if (optionsMenu.activeSelf == true)
        {
            x = 0;
            y = 0;
        }

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

        if (!up && !down && !right && !left && !Input.anyKey)
        {
            inputAvailable = true;
        }
        
        if (_inputActions.Gameplay.Enter.IsPressed() && optionsMenu.activeSelf == true)
        {
            inputAvailable = false;
        }
        

        if (!inputAvailable)
        {
            return;
        }

        if (left || up)
        {
            audioSource.Play();
            curr = Mathf.Max(0, curr - 1);
            inputAvailable = false;
        }
        else if (right || down)
        {
            audioSource.Play();
            curr = Mathf.Min(buttons.Count - 1, curr + 1);
            inputAvailable = false;
        }

        if (_inputActions.Gameplay.Enter.IsPressed() && optionsMenu.activeSelf == false)
        {
            audioSourceCHOOSE.Play();
            //WAIT HERE

            if (buttons[curr].name == "start")
            {
                
                SceneManager.LoadScene("LevelSelect");
            }
            else if (buttons[curr].name == "exit")
            {

                SceneManager.LoadScene("SplashScreen");
            }
            else if (buttons[curr].name == "options") 
            {

                optionsMenu.SetActive(true);
            }
        }

        Vector3 buttonPosition = buttons[curr].transform.position;
        pickerScript.startingPosition = buttonPosition - new Vector3(150, 0, 0);
    }
    
}
