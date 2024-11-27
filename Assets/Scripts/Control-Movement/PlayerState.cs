using System.Collections;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class PlayerState : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    
    public GameObject standardIdlePrefab, standardWalkPrefab, standardJumpPrefab;
    public GameObject gravIdlePrefab, gravWalkPrefab, gravJumpPrefab;
    public GameObject teleportIdlePrefab, teleportAimPrefab, teleportWalkPrefab, teleportJumpPrefab;
    public GameObject wallJumpIdlePrefab, wallJumpWalkPrefab, wallJumpJumpPrefab, wallJumpRunPrefab, wallJumpWalljumpPrefab;
    
    // private bool _isGrounded = false;
    // private float _groundCheckDistance = 0.5f;
    // private float _rayOffset = 0.9f;

    private Rigidbody _playerRigidbody;
    private GameObject _currentActivePrefab;

    private bool _isActionInProgress = false;
    private float _velocity;

    private AudioSource[] _audioSources;
    private int currentPass = 0;
    
    private portal portal;
    
    private PlayerMovement playerMovement;

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
        _playerRigidbody = GetComponent<Rigidbody>();

        portal = FindObjectOfType<portal>();
        
        playerMovement = GetComponent<PlayerMovement>();
        
        SetActivePrefab(standardIdlePrefab);
        _audioSources = GetComponents<AudioSource>();
    }

    void Update()
    {
        // CheckGrounded();

        if (!_isActionInProgress)
        {
            _velocity = _playerRigidbody.velocity.magnitude; 
            float _velocityXZ = Mathf.Abs(_playerRigidbody.velocity.x) + Mathf.Abs(_playerRigidbody.velocity.z);
            float _velocityY = Mathf.Abs(_playerRigidbody.velocity.y);
            
            Vector2 _moveInput = _inputActions.Gameplay.Move.ReadValue<Vector2>();
            
            if (playerMovement._userWallJumped)
            {
                GameObject idlePrefab = GetCurrentAbilityPrefab("Idle");
                SetActivePrefab(idlePrefab);
                GameObject walljumpPrefab = GetCurrentAbilityPrefab("WallJump");
                SetActivePrefab(walljumpPrefab);
                StopWalkAudio();
            }
            // else if ((_playerRigidbody.velocity.y > 0 || _inputActions.Gameplay.Jump.IsPressed()) && !playerMovement._isTP)
            else if (((playerMovement._isGrounded && _inputActions.Gameplay.Jump.IsPressed()) || playerMovement._userJumped) && !playerMovement._isTP)
            {
                GameObject idlePrefab = GetCurrentAbilityPrefab("Idle");
                SetActivePrefab(idlePrefab);
                GameObject jumpPrefab = GetCurrentAbilityPrefab("Jump");
                SetActivePrefab(jumpPrefab);
                StopWalkAudio();
            }
            else if (playerMovement._isGrounded && playerMovement._running && _velocityXZ > 7f && _moveInput.magnitude > 0.1f)
            {
                GameObject runPrefab = GetCurrentAbilityPrefab("Run");
                SetActivePrefab(runPrefab);
                PlayWalkAudio();
            }
            else if (playerMovement._isGrounded && _velocityXZ > 7f && _moveInput.magnitude > 0.1f)
            {
                GameObject walkPrefab = GetCurrentAbilityPrefab("Walk");
                SetActivePrefab(walkPrefab);
                PlayWalkAudio();
            }
            else if (playerMovement._isTP && Throwing.isAiming)
            {
                GameObject aimPrefab = GetCurrentAbilityPrefab("Aim");
                SetActivePrefab(aimPrefab);
                StopWalkAudio();
            }
            else if (playerMovement._isGrounded && _velocityXZ < 7f && _velocityY < 0.5f)
            {
                GameObject idlePrefab = GetCurrentAbilityPrefab("Idle");
                SetActivePrefab(idlePrefab);
                StopWalkAudio();
            }
            
            // OLD CONDITIONS
            // if (playerMovement._userWallJumped)
            // {
            //     GameObject walljumpPrefab = GetCurrentAbilityPrefab("WallJump");
            //     StopWalkAudio();
            //     StartCoroutine(SwitchToPrefabForDuration(walljumpPrefab, 1.5f));
            // }
            // else if (playerMovement._isGrounded && _inputActions.Gameplay.Jump.IsPressed() && !playerMovement._isTP)
            // {
            //     GameObject jumpPrefab = GetCurrentAbilityPrefab("Jump");
            //     StopWalkAudio();
            //     StartCoroutine(SwitchToPrefabForDuration(jumpPrefab, 0.967f));
            // }
            // else if (playerMovement._isGrounded && playerMovement._running && _velocity > 5.5f)
            // {
            //     GameObject runPrefab = GetCurrentAbilityPrefab("Run");
            //     SetActivePrefab(runPrefab);
            //     PlayWalkAudio();
            // }
            // else if (playerMovement._isGrounded && _velocity > 5.5f)
            // {
            //     // Debug.Log(_velocity);
            //     GameObject walkPrefab = GetCurrentAbilityPrefab("Walk");
            //     SetActivePrefab(walkPrefab);
            //     PlayWalkAudio();
            // }
            // else
            // {
            //     GameObject idlePrefab = GetCurrentAbilityPrefab("Idle");
            //     SetActivePrefab(idlePrefab);
            //     StopWalkAudio();
            // }
        }
    }

    private GameObject GetCurrentAbilityPrefab(string actionType)
    {
        switch (portal.currentAbility)
        {
            case 2: // Teleport
                return actionType == "Idle" ? teleportIdlePrefab
                     : actionType == "Walk" ? teleportWalkPrefab
                     : actionType == "Aim" ? teleportAimPrefab
                     : teleportJumpPrefab;

            case 3: // Gravity
                return actionType == "Idle" ? gravIdlePrefab
                     : actionType == "Walk" ? gravWalkPrefab
                     : gravJumpPrefab;

            case 1: // Wall Jump
                return actionType == "Idle" ? wallJumpIdlePrefab
                     : actionType == "Walk" ? wallJumpWalkPrefab
                     : actionType == "Jump" ? wallJumpJumpPrefab
                     : actionType == "Run" ? wallJumpRunPrefab
                     : wallJumpWalljumpPrefab;

            default: // Standard
                return actionType == "Idle" ? standardIdlePrefab
                     : actionType == "Walk" ? standardWalkPrefab
                     : standardJumpPrefab;
        }
    }

    private void SetActivePrefab(GameObject prefabToActivate)
    {
        if (_currentActivePrefab != prefabToActivate)
        {
            if (_currentActivePrefab != null)
            {
                _currentActivePrefab.SetActive(false);
            }

            prefabToActivate.SetActive(true);
            _currentActivePrefab = prefabToActivate;
        }
    }

    private IEnumerator SwitchToPrefabForDuration(GameObject prefab, float duration)
    {
        _isActionInProgress = true;
        SetActivePrefab(prefab);
        yield return new WaitForSeconds(duration);
        
        _isActionInProgress = false;

        _velocity = _playerRigidbody.velocity.magnitude;
        if (_velocity > 0.1f)
        {
            GameObject walkPrefab = GetCurrentAbilityPrefab("Walk");
            SetActivePrefab(walkPrefab);
        }
        else
        {
            GameObject idlePrefab = GetCurrentAbilityPrefab("Idle");
            SetActivePrefab(idlePrefab);
        }
    }

    // private void CheckGrounded()
    // {
    //     _isGrounded = Physics.Raycast(transform.position - _rayOffset * transform.up,
    //         -transform.up, _groundCheckDistance);
    // }
    //
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(transform.position - _rayOffset * transform.up,
    //         (transform.position - _rayOffset * transform.up) + -transform.up * _groundCheckDistance);
    // }

    private void PlayWalkAudio()
    {
        if (!_audioSources[0].isPlaying)
        {
            _audioSources[0].Play();
        }
    }

    private void StopWalkAudio()
    {
        if (_audioSources[0].isPlaying)
        {
            _audioSources[0].Stop();
        }
    }
}
