using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{   
    [Header("Components")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform guardTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform prefabFieldOfView;
    [SerializeField] private float FieldOfViewAngle;
    [SerializeField] private float viewDistance;
    //private FieldOfView FieldOfView;
    private FOV FOVDetection;
    private Vector3 previousPosition;
    private CoinToss coinToss;

    public bool isFacingRight = true;
    public bool isWalkingRight;
    public bool isWalkingLeft;
    public bool isWalkingUp;
    public bool isWalkingDown;

    [Header("A* Chasing")]
    private GridController gridController;
    private bool isChasing = false;
    private List<Tile> currentPath;
    
    [Header("Patrolling")]
    [SerializeField] private Transform[] patrolPoints;
    private int targetPoint;

    

    void Awake()
    {
        Physics2D.queriesStartInColliders = false;

        gridController = FindObjectOfType<GridController>();
        coinToss = FindObjectOfType<CoinToss>();

        targetPoint = 0;

        if (gridController == null)
        {
            Debug.LogError("GridController not found.");
        }

        FOVDetection = GetComponent<FOV>();

        /*FieldOfView = Instantiate(prefabFieldOfView, null).GetComponent<FieldOfView>();
        FieldOfView.SetFoV(FieldOfViewAngle);
        FieldOfView.SetViewDistance(viewDistance);*/
    }

    private void Update()
    {

        if (!isChasing)
        {
            Patrol();
        }
        else
        {
            // Optionally, you can add a condition to stop chasing if the player is no longer visible.
            if (!FOVDetection.canSeePlayer)
            {
                StopChasing();
            }

        }

        SetAnimBools();

    }

    void SetAnimBools()
    {
        Vector3 lookDir = GetLookDirection();
        float inputHorizontal = lookDir.x;
        float inputVertical = lookDir.y;
        //Debug.Log("(" + lookDir.x + ", " + lookDir.y + ", " + lookDir.z + ")");
        if (Mathf.Abs(inputHorizontal) == 0.0f && Mathf.Abs(inputVertical) == 0.0f)
        {
            isWalkingRight = false;
            isWalkingLeft = false;
            isWalkingUp = false;
            isWalkingDown = false;
        }
        else if (inputHorizontal > 0.1f)
        {
            isWalkingRight = true;
            isWalkingUp = false;
            isWalkingDown = false;
        }
        else if (inputHorizontal < -0.1f)
        {
            isWalkingLeft = true;
            isWalkingUp = false;
            isWalkingDown = false;
        }
        else if (inputVertical > 0.1f)
        {
            isWalkingUp = true;
            isWalkingLeft = false;
            isWalkingRight = false;
        }
        else if (inputVertical < -0.1f)
        {
            isWalkingDown = true;
            isWalkingLeft = false;
            isWalkingRight = false;
        }

        Flip(inputHorizontal, inputVertical);

    }

    private void Flip(float inputHorizontal, float inputVertical)
    {
        if((isFacingRight && inputHorizontal < 0f) || (!isFacingRight && inputHorizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void StartChasing()
    {
        if (!isChasing)
        {
            isChasing = true;
            StartCoroutine(AStarPathfinding());
        }
    }

    public void StopChasing()
    {
        isChasing = false;
        // Implement any logic to stop chasing here
        Debug.Log("Stopping chasing.");

        // Clear the path
        ClearPath();
    }

    void Patrol()
    {
        if (Vector2.Distance(transform.position, patrolPoints[targetPoint].position) < 0.2f)
        {
            IncreaseTargetInt();
        }

        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[targetPoint].position, moveSpeed * Time.deltaTime);      
    }

    void IncreaseTargetInt()
    {
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }

    IEnumerator AStarPathfinding()
    {
        while (isChasing)
        {
            Vector2 guardPosition = guardTransform.position;
            Vector2 playerPosition = playerTransform.position;

            // Find path
            currentPath = gridController.FindPath(guardPosition, playerPosition);

            if (currentPath != null && currentPath.Count > 0)
            {
                MoveAlongPath();
            }
            else
            {
                // If the path is empty or null, stop the guard
                StopChasing();
            }

            yield return null;
        }
    }

    void MoveAlongPath()
    {
        if (currentPath != null && currentPath.Count > 0)
        {
            Vector2 direction = (currentPath[0].worldPosition - (Vector2)guardTransform.position).normalized;
            rb.velocity = direction * moveSpeed;

            // Check if the guard is close enough to the current node in the path
            float distanceToNode = Vector2.Distance(guardTransform.position, currentPath[0].worldPosition);
            if (distanceToNode < 0.3f)
            {
                // Remove the reached node from the path
                currentPath.RemoveAt(0);
                print(currentPath.Count);
            }
        }
    }

    void ClearPath()
    {
        currentPath = null;
        rb.velocity = Vector2.zero;
    }

    public Vector3 GetAimDirection()
    {
        // Calculate the movement direction
        Vector3 movementDirection = (transform.position - previousPosition).normalized;

        // Update the previous position for the next frame
        previousPosition = transform.position;

        return movementDirection;
    }

    public Vector3 GetLookDirection()
    {
        return (transform.position - previousPosition).normalized;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Coin") && coinToss.speed > 0f)
        {
            Debug.Log("Heard coin");
        }
    }


    
}