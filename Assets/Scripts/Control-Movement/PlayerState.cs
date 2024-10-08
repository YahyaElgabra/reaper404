using System.Collections;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public GameObject idlePrefab;
    public GameObject runPrefab;
    public GameObject jumpPrefab;
    // public GameObject attackPrefab;
    
    private bool _isGrounded = false;
    private float _groundCheckDistance = 0.5f;
    private float _rayOffset = 0.9f;
    
    private Rigidbody _playerRigidbody;
    private GameObject _currentActivePrefab;
    
    private bool _isActionInProgress = false;
    private float _velocity;
    
    private AudioSource[] _audioSources;
    
    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        SetActivePrefab(idlePrefab);
        
        _audioSources = GetComponents<AudioSource>();
    }

    void Update()
    {
        CheckGrounded();
        
        if (!_isActionInProgress)
        {
            _velocity = _playerRigidbody.velocity.magnitude;
            
            
            if (_isGrounded && Input.GetButton("Jump"))
            {
                StartCoroutine(SwitchToPrefabForDuration(jumpPrefab, 0.633f));
            }
            else if (_isGrounded && _velocity > 3f)
            {
                SetActivePrefab(runPrefab);
                PlayRunAudio();
            }
            // else if (Input.GetKeyDown(KeyCode.LeftShift))
            // {
            //     StartCoroutine(SwitchToPrefabForDuration(attackPrefab, 1.533f));
            // }
            else
            {
                SetActivePrefab(idlePrefab);
                StopRunAudio();
            }
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
            SetActivePrefab(runPrefab);
        }
        else
        {
            SetActivePrefab(idlePrefab);
        }
    }
    
    private void CheckGrounded()
    {
        _isGrounded = Physics.Raycast(transform.position - _rayOffset * transform.up, 
            -transform.up, _groundCheckDistance);
        
        // Debug.Log("Postion: " + transform.position);
        // Debug.Log("IsGrounded: " + _isGrounded);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - _rayOffset * transform.up, 
            (transform.position - _rayOffset * transform.up) + -transform.up * _groundCheckDistance);
    }
    
    // Method to play the running sound
    private void PlayRunAudio()
    {
        if (!_audioSources[0].isPlaying)
        {
            // _audioSource.clip = runAudioClip;
            // _audioSource.loop = true;
            _audioSources[0].Play();
        }
    }

    // Method to stop the running sound
    private void StopRunAudio()
    {
        if (_audioSources[0].isPlaying)
        {
            _audioSources[0].Stop();
        }
    }
}

