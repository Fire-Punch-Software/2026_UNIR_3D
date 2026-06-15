using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed;
    public float runningSpeed;
    private float currentMovingSpeed;
    public float turningSpeed = 300f;
    private float characterHealth = 40f;
    public float presentHealth;

    [Header("Destination Var")]
    public Animator animator;
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;
    private bool movingForward = true;

    [Header("Enemy AI")]
    GameObject playerBody;
    public LayerMask playerLayer;
    public float visionRadius;
    public float chasingRadius;
    public bool playerInVisionRadius;
    public bool playerInChasingRadius;

    [Header("Character Controller and Gravity")]
    public CharacterController characterController;
    public float gravity = 9.81f;
    private Vector3 velocity;
    public bool isAlerted = false;

    public GameObject warningImage;
    public GameObject alertImage;
    private bool blinded = false;

    void Start()
    {
        presentHealth = characterHealth;
        currentMovingSpeed = movingSpeed;
        playerBody = GameObject.Find("Player");
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!blinded)
        {
            playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
            playerInChasingRadius = Physics.CheckSphere(transform.position, chasingRadius, playerLayer);

            if (playerInVisionRadius)
            {
                warningImage.SetActive(true);
                alertImage.SetActive(false);
            }

            if (!isAlerted)
            {
                Patrol();
            }

            if (isAlerted && playerInChasingRadius && playerInVisionRadius)
            {
                ChasePlayer();
            }

            if (isAlerted)
            {
                warningImage.SetActive(false);
                alertImage.SetActive(true);

                if (!playerInVisionRadius)
                {
                    isAlerted = false;
                    alertImage.SetActive(false);
                }
            }
        }
    }

    public void AlertEnemy()
    {
        isAlerted = true;
    }

    private void Patrol()
    {
        if (waypoints.Count == 0) return;

        currentMovingSpeed = movingSpeed;
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 directionToWaypoint = (targetWaypoint.position - transform.position).normalized;
        Vector3 moveVector = directionToWaypoint * currentMovingSpeed * Time.deltaTime;

        characterController.Move(moveVector);

        Vector3 lookDirection = new Vector3(directionToWaypoint.x, 0, directionToWaypoint.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), turningSpeed * Time.deltaTime);

        animator.SetBool("Run", false);
        animator.SetBool("Walk", true);
        animator.SetBool("Die", false);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (movingForward)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Count)
                {
                    currentWaypointIndex = waypoints.Count - 1;
                    movingForward = false;
                }
            }
            else
            {
                currentWaypointIndex--;
                if (currentWaypointIndex < 0)
                {
                    currentWaypointIndex = 0;
                    movingForward = true;
                }
            }
        }
    }

    void ChasePlayer()
    {
        currentMovingSpeed = runningSpeed;
        Vector3 directionToPlayer = (playerBody.transform.position - transform.position).normalized;
        Vector3 moveVector = directionToPlayer * currentMovingSpeed * Time.deltaTime;

        characterController.Move(moveVector);
        
        Vector3 lookDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), turningSpeed * Time.deltaTime);

        animator.SetBool("Run", true);
        animator.SetBool("Walk", false);
        animator.SetBool("Die", false);
    }

    void CharacterDie()
    {
        currentMovingSpeed = 0f;

        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Die", true);

        this.enabled = false;

        warningImage.SetActive(false);
        alertImage.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!blinded)
        {
            GameObject collidedObject = collision.gameObject;

            if (collidedObject.CompareTag("Player"))
            {
                blinded = true;
                warningImage.SetActive(false);
                alertImage.SetActive(false);

                animator.SetBool("Run", false);
                animator.SetBool("Walk", false);
                animator.SetBool("Die", false);

                animator.SetTrigger("Kill");
                collidedObject.GetComponent<AnimatorManager>().Die();

                
                GameManager gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.GameOver();
                }

                this.enabled = false;

            }
        }
    }
}
