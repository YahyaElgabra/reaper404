using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerMovement playerMovement;

    private float _rotDuration = 0.2f;
    private float _lastRotTime;
    public int charges;
    AbilitiesUI abilitiesUI;
    public GameObject arrow;

    private AudioSource[] _audioSources;

    private bool _isGravDisabled;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        _audioSources = GetComponents<AudioSource>();
        _lastRotTime = Time.time - _rotDuration;
        GameObject abilitiesObject = GameObject.FindGameObjectWithTag("AbilitiesUI");
        if (abilitiesObject != null)
        {
            abilitiesUI = abilitiesObject.GetComponent<AbilitiesUI>();
        }
        _isGravDisabled = false;
    }

    private void Update()
    {
        Vector3 lookDirection = FindSide(0);
        Vector3 rightDirection = FindSide(1);
        Vector3 upDirection = Vector3.Cross(rightDirection, lookDirection).normalized;
        arrow.transform.rotation = Quaternion.LookRotation(lookDirection, upDirection);

        if (playerMovement.GetIsGrounded() && Time.time - _lastRotTime > _rotDuration)
        {
            _isGravDisabled = false;
        }
        if (playerMovement._isGrav)
        {
            arrow.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }
    }

    public void RotateGravity(int side)
    {
        if (charges > 0 && !_isGravDisabled)
        {
            _isGravDisabled = true;

            _lastRotTime = Time.time;

            if (side == 1)
            {
                playerMovement.gravityDirection = FindSide(1);
            }
            else
            {
                playerMovement.gravityDirection = FindSide(-1);
            }
            FlipCharacterModel();
            PlayGravityAudio();
            charges--;
            if (abilitiesUI != null)
            {
                abilitiesUI.updateCharges(charges);
            }
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private void FlipCharacterModel()
    {
        Quaternion rotation = Quaternion.FromToRotation(-transform.up, playerMovement.gravityDirection);
        StartCoroutine(SmoothRotate(rotation * transform.rotation));
    }

    private IEnumerator SmoothRotate(Quaternion targetRotation)
    {   
        float timeElapsed = 0f;

        while (timeElapsed < _rotDuration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, timeElapsed / _rotDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }


    private Vector3 FindSide(int scalar)
    {
        // returns the closest unit vector to "right" (or left if the input is -1), negative means left and positive means right
        Vector3[] directions = new Vector3[]
{
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0)
        };

        Vector3 closestDirection = Vector3.zero;
        float closestAngle = float.MaxValue;

        if (scalar == 0)
        {
            foreach (Vector3 dir in directions)
            {
                float angle = Vector3.Angle(transform.forward, dir);
                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    closestDirection = dir;
                }
            }
        }
        else
        {
            foreach (Vector3 dir in directions)
            {
                float angle = Vector3.Angle(scalar * transform.right, dir);
                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    closestDirection = dir;
                }
            }
        }

        return closestDirection;
    }
    private void PlayGravityAudio()
    {
        _audioSources[1].Play();
    }
}
