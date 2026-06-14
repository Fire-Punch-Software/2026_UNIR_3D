using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Script References")]
    InputManager inputManager;

    [Header("Movement Settings")]
    Vector3 moveDirection;
    public Transform camObject;
    Rigidbody rb;

    public float walkingSpeed = 5f;
    public float runningSpeed = 10f;
    public float rotationSpeed = 12f;

    [Header("Movement Flags")]
    public bool isMoving;
    public bool isRunning;
    public bool isGrounded;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public float fallSpeed = 5f;

    void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
        ApplyGravity();
    }

    void HandleMovement()
    {
        moveDirection = camObject.forward * inputManager.verticalInput;
        moveDirection += camObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        if (isRunning)
        {
            moveDirection = moveDirection * runningSpeed;
        }
        else
        {
            if (inputManager.moveAmount > 0.5f) {
                moveDirection = moveDirection * walkingSpeed;
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }

        Vector3 movementVelocity = moveDirection;
        movementVelocity.y = rb.velocity.y;
        rb.velocity = movementVelocity;
    }

    void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = camObject.forward * inputManager.verticalInput;
        targetDirection += camObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0f;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.y += gravity * fallSpeed * Time.deltaTime;
            rb.velocity = currentVelocity;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

}
