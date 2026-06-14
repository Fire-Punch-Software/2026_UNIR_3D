using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    public Transform playerTransform;

    public Transform cameraPivot;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    [Header("Camera Movement and Rotation")]
    public float cameraFollowSpeed = 0.1f;
    public float cameraLookSpeed = 0.1f;
    public float cameraPivotSpeed = 0.1f;
    public float lookAngle;
    public float pivotAngle;
    public float minimumPivotAngle = -30f;
    public float maximumPivotAngle = 30f;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerTransform = FindObjectOfType<PlayerManager>().transform;

        inputManager = FindObjectOfType<InputManager>();

    }

    void Update()
    {
        //inputManager = FindObjectOfType<InputManager>();
        //playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
    }

    void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, playerTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (inputManager.cameraInputX * cameraFollowSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);

        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        playerTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }
}
