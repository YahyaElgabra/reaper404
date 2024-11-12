using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Throwing : MonoBehaviour
{
    private PlayerInputActions _inputActions;
    private float _udInput;
    private float _ewInput;
    
    public Transform playerTransform;
    private bool isHeld = false;
    private Vector3 offset = new Vector3(1f, 2f, 1f);
    
    private Rigidbody rb;
    private Collider objectCollider;
    
    public GameObject throwablePrefab;
    private GameObject currentThrowable; 
    private Throwable currentThrowableScript;
    
    private float throwForce = 15f;
    private bool isThrown = false;
    
    public static bool isAiming = false;
    private LineRenderer trajectoryLine;
    private int trajectoryPointsCount = 50;
    private float timeBetweenPoints = 0.1f;

    private float aimHorizontal;
    private float aimVertical;
    private Vector3 aimDirection;
    
    private float _maxAimAngleX = 45f;
    private float _minAimAngleX = -45f;
    private float _maxAimAngleY = 85f;
    private float _minAimAngleY = -25f;
    private float _aimSensX = 0.01f;
    private float _aimSensY = 0.01f;
    
    private Camera playerCamera;
    private MonoBehaviour cameraController;

    AbilitiesUI abilitiesUI;
    public int charges;

    private PlayerMovement playerMovement;
    
    public GameObject endpointPrefab;
    private GameObject endpointInstance;
    
    public Material circleMaterial;
    private List<LineRenderer> circleRenderers = new List<LineRenderer>();
    
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
        playerTransform = this.transform;
        GameObject playerObject = GameObject.FindWithTag("Player");
        
        playerCamera = GetComponentInChildren<Camera>();
        
        cameraController = playerCamera.GetComponent<MonoBehaviour>();
        
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        GameObject abilitiesObject = GameObject.FindGameObjectWithTag("AbilitiesUI");
        if (abilitiesObject != null)
        {
            abilitiesUI = abilitiesObject.GetComponent<AbilitiesUI>();
        }
        
        if (endpointPrefab != null)
        {
            endpointInstance = Instantiate(endpointPrefab);
            endpointInstance.SetActive(false);
        }
    }
    
    void Update()
    {
        if (_inputActions.Gameplay.ThrowHold.IsPressed() && 
            !isHeld && charges > 0 && !isThrown && playerMovement._isTP)
        {
            SpawnThrowableObject();
        }
        
        if (isHeld)
        {
            currentThrowable.transform.position = playerTransform.position + playerTransform.rotation * offset;
            
            if (_inputActions.Gameplay.ThrowHold.IsPressed())
            {
                EnterAimMode();
            }
            
            if (_inputActions.Gameplay.ThrowRelease.WasPerformedThisFrame() && isAiming)
            {
                ThrowObject();
                if (charges == 0)
                {
                    StartCoroutine(OutOfCharge());
                }
            }
        }
    }
    
    IEnumerator OutOfCharge()
    {
        float elapsedTime = 0f;

        while (5.5f > elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SpawnThrowableObject()
    {
        if (charges > 0)
        {
            currentThrowable = Instantiate(throwablePrefab, playerTransform.position + playerTransform.rotation * offset, Quaternion.identity);

            currentThrowableScript = currentThrowable.GetComponent<Throwable>();
            
            if (currentThrowableScript != null)
            {
                currentThrowableScript.Initialize(this);
            }
            
            isHeld = true;
            isThrown = false;
            
            charges--;
            if (abilitiesUI != null)
            {
                abilitiesUI.updateCharges(charges);
            }
        }
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
        Vector2 _lookInput = _inputActions.Gameplay.Look.ReadValue<Vector2>();
        
        _udInput = _lookInput.y;
        _ewInput = _lookInput.x;
        
        aimHorizontal = - - _ewInput;
        aimVertical = - _udInput;
        
        Vector3 right = playerTransform.right;
        Vector3 up = playerTransform.up;
        
        aimDirection += right * (aimHorizontal * _aimSensX);
        aimDirection -= up * (aimVertical * _aimSensY);
        
        aimDirection.y = Mathf.Clamp(aimDirection.y, -1f, 1f);
        
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
        trajectoryLine = currentThrowable.GetComponent<LineRenderer>();
        
        if (trajectoryLine == null) return;

        trajectoryLine.positionCount = trajectoryPointsCount;
        
        Vector3 startPos = playerTransform.position + playerTransform.rotation * offset;
        Vector3 startVelocity = aimDirection * throwForce;

        Vector3 previousPoint = startPos;

        for (int i = 0; i < trajectoryPointsCount; i++)
        {
            float t = i * timeBetweenPoints;
            Vector3 pointPosition = startPos + t * startVelocity;
            pointPosition.y = startPos.y + (startVelocity.y * t) + (0.5f * (Physics.gravity.y - 0.1f) * t * t);
            
            if (Physics.Raycast(previousPoint, pointPosition - previousPoint, out RaycastHit hit, (pointPosition - previousPoint).magnitude))
            {
                trajectoryLine.SetPosition(i, hit.point);
                trajectoryLine.positionCount = i + 1;
                
                float surfaceAngle = Vector3.Angle(hit.normal, Vector3.up);
                bool isFlatSurface = surfaceAngle < 45f;
                bool isFinishSurface = hit.collider.CompareTag("Finish");
                
                if (isFlatSurface || isFinishSurface)
                {
                    if (endpointInstance != null)
                    {
                        endpointInstance.transform.position = hit.point + Vector3.up * 0.5f;
                        endpointInstance.SetActive(true); 
                    }
                    
                    DrawConcentricCircles(hit.point);
                }
                else
                {
                    if (endpointInstance != null)
                    {
                        endpointInstance.SetActive(false);
                    }
                    
                    ClearConcentricCircles();
                }
                
                break;
            }
            else
            {
                trajectoryLine.SetPosition(i, pointPosition);
                
                if (endpointInstance != null)
                {
                    endpointInstance.SetActive(false);
                }
                
                ClearConcentricCircles();
            }
            previousPoint = pointPosition;
        }
    }
    
    private void DrawConcentricCircles(Vector3 center)
    {
        int numberOfCircles = 3;
        float initialRadius = 0.5f;
        float radiusIncrement = 1.0f;
        int segments = 30;
        
        ClearConcentricCircles();

        for (int i = 0; i < numberOfCircles; i++)
        {
            LineRenderer lineRenderer = new GameObject($"Circle_{i}").AddComponent<LineRenderer>();
            lineRenderer.positionCount = segments + 1;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.useWorldSpace = false;
            lineRenderer.material = circleMaterial;
            
            float radius = initialRadius + i * 2 * radiusIncrement;
            Vector3[] positions = new Vector3[segments + 1];
        
            for (int j = 0; j <= segments; j++)
            {
                float angle = j * 2 * Mathf.PI / segments;
                positions[j] = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            }

            lineRenderer.SetPositions(positions);
            
            lineRenderer.transform.position = center;
            circleRenderers.Add(lineRenderer);
        }
    }

    private void ClearConcentricCircles()
    {
        foreach (var lineRenderer in circleRenderers)
        {
            if (lineRenderer != null)
            {
                Destroy(lineRenderer.gameObject);
            }
        }
        circleRenderers.Clear();
    }
    
    private void ThrowObject()
    {
        if (isHeld && currentThrowableScript != null)
        {
            isHeld = false;
            isThrown = true;
            isAiming = false;
            
            trajectoryLine.positionCount = 0;
            
            if (endpointInstance != null)
                endpointInstance.SetActive(false);
            
            ClearConcentricCircles();
            
            if (cameraController != null)
                cameraController.enabled = true;
            
            currentThrowableScript.Throw(aimDirection, throwForce);
        }
        
        isHeld = false;
    }
    
    public void TeleportPlayerAndDestroy(GameObject throwable)
    {
        playerTransform.position = throwable.transform.position + Vector3.up * 0.5f;
    
        Destroy(throwable);
        isThrown = false;
    }
    
    public void OnThrowableHitDeath(GameObject throwable)
    {
        Destroy(throwable);
        isThrown = false;
    }
}
