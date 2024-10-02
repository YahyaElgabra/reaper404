using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{
    public Transform playerTransform;
    private bool isHeld = false;
    private Vector3 offset = new Vector3(1f, 2f, -1f);
    
    private Rigidbody rb;
    private Collider objectCollider;
    float timeToMove = 1f;
    
    private float throwForce = 15f;
    private bool isThrown = false;
    
    public static bool isAiming = false;
    private LineRenderer trajectoryLine;
    private int trajectoryPointsCount = 30;
    private float timeBetweenPoints = 0.1f;

    private float aimHorizontal;
    private float aimVertical;
    private Vector3 aimDirection;
    
    private float _maxAimAngleX = 45f;
    private float _minAimAngleX = -45f;
    private float _maxAimAngleY = 75f;
    private float _minAimAngleY = 0f;
    private float _aimSensX = 0.05f;
    private float _aimSensY = 0.1f;
    
    private Camera playerCamera;
    private MonoBehaviour cameraController;

    
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        
        trajectoryLine = GetComponent<LineRenderer>();
        
        playerCamera = playerObject.GetComponentInChildren<Camera>();
        
        cameraController = playerCamera.GetComponent<MonoBehaviour>();
    }
    
    void Update()
    {
        if (isHeld)
        {
            transform.position = playerTransform.position + playerTransform.rotation * offset;
            
            if (Input.GetKey(KeyCode.C) || Input.GetButton("Fire3"))
            {
                EnterAimMode();
            }
            
            if ((Input.GetKeyUp(KeyCode.C) || Input.GetButtonUp("Fire3")) && isAiming)
            {
                ThrowObject();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isHeld)
        {
            StartCoroutine(MoveToPlayer());
        }
        
        if (isThrown && !collision.gameObject.CompareTag("Player"))
        {
            TeleportPlayerAndDestroy();
        }
    }

    private IEnumerator MoveToPlayer()
    {
        isHeld = true;
        
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        if (objectCollider != null)
        {
            objectCollider.enabled = false;
        }
        
        Vector3 startPos = transform.position;

        for (float t = 0; t < 1; t += Time.deltaTime / timeToMove)
        {
            Vector3 targetPos = playerTransform.position + playerTransform.rotation * offset;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
        
        transform.position = playerTransform.position + playerTransform.rotation * offset;
    }
    
    private void EnterAimMode()
    {
        if (!isAiming)
        {
            isAiming = true;
            
            if (cameraController != null)
                cameraController.enabled = false;
            
            aimDirection = playerTransform.forward;
            
            float angle = 45f;
            float throwHeight = Mathf.Tan(angle * Mathf.Deg2Rad);
            aimDirection.y += throwHeight;
            aimDirection.Normalize();
        }
        
        UpdateAimDirection();
        ShowTrajectory();
    }
    
    private void UpdateAimDirection()
    {
        aimHorizontal = Input.GetAxis("RJoy X");
        aimVertical = Input.GetAxis("RJoy Y");
        
        Vector3 right = playerTransform.right;
        Vector3 up = playerTransform.up;
        
        aimDirection += right * (aimHorizontal * _aimSensX);
        aimDirection -= up * (aimVertical * _aimSensY);
        
        float pitchY = Mathf.Asin(aimDirection.y) * Mathf.Rad2Deg;
        pitchY = Mathf.Clamp(pitchY, _minAimAngleY, _maxAimAngleY);
        
        aimDirection.y = Mathf.Sin(pitchY * Mathf.Deg2Rad);
        
        Vector3 flatDirection = new Vector3(aimDirection.x, 0, aimDirection.z);
        float horizontalAngle = Vector3.SignedAngle(playerTransform.forward, flatDirection, Vector3.up);
        
        horizontalAngle = Mathf.Clamp(horizontalAngle, _minAimAngleX, _maxAimAngleX);
        
        Quaternion rotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
        flatDirection = rotation * playerTransform.forward;
        
        aimDirection.x = flatDirection.x;
        aimDirection.z = flatDirection.z;
    }

    private void ShowTrajectory()
    {
        if (trajectoryLine == null) return;

        trajectoryLine.positionCount = trajectoryPointsCount;
        
        Vector3 startPos = playerTransform.position + playerTransform.rotation * offset;
        Vector3 startVelocity = aimDirection * throwForce;

        for (int i = 0; i < trajectoryPointsCount; i++)
        {
            float t = i * timeBetweenPoints;
            Vector3 pointPosition = startPos + t * startVelocity;
            pointPosition.y = startPos.y + (startVelocity.y * t) + (0.5f * Physics.gravity.y * t * t);

            trajectoryLine.SetPosition(i, pointPosition);
        }
    }
    
    private void ThrowObject()
    {
        if (isHeld)
        {
            isHeld = false;
            isThrown = true;
            isAiming = false;
            
            trajectoryLine.positionCount = 0;
            
            if (cameraController != null)
                cameraController.enabled = true;

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                
                rb.AddForce(aimDirection * throwForce, ForceMode.Impulse);
                
            }
            
            if (objectCollider != null)
            {
                objectCollider.enabled = true;
            }
        }
        
        isHeld = false;
    }

    private void TeleportPlayerAndDestroy()
    {
        playerTransform.position = transform.position;
        Destroy(gameObject);
    }
}
