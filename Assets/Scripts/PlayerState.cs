using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public GameObject idlePrefab;
    public GameObject runPrefab;
    public GameObject jumpPrefab;
    public GameObject attackPrefab;

    private Rigidbody _playerRigidbody;
    private GameObject _currentActivePrefab;
    
    private bool _isActionInProgress = false;
    private float _velocity;
    void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        SetActivePrefab(idlePrefab);
    }

    void Update()
    {
        if (!_isActionInProgress)
        {
            _velocity = _playerRigidbody.velocity.magnitude;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(SwitchToPrefabForDuration(jumpPrefab, 1.033f));
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(SwitchToPrefabForDuration(attackPrefab, 1.533f));
            }
            else if (_velocity > 0.5f)
            {
                SetActivePrefab(runPrefab);
            }
            else
            {
                SetActivePrefab(idlePrefab);
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
}
