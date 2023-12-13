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
    private FieldOfView FieldOfView;
    private FOV FOVDetection;
    private Vector3 previousPosition;
    private CoinToss coinToss;

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
        Patrol();

        SetAnimBools(rb.velocity.x, rb.velocity.y);

        /*FieldOfView.SetOrigin(transform.position);
        FieldOfView.SetAimDirection(GetAimDirection());*/

    } 

    void SetAnimBools(float inputHorizontal, float inputVertical)
    {
        if(inputHorizontal > 0){
            anim.SetBool("isWalkingRight", true);
        } else{
            anim.SetBool("isWalkingRight", false);
        }
        if(inputHorizontal < 0){
            anim.SetBool("isWalkingLeft", true);
        } else{
            anim.SetBool("isWalkingLeft", false);
        }
        if(inputVertical > 0 && inputHorizontal == 0){
            anim.SetBool("isWalkingUp", true);
        } else{
            anim.SetBool("isWalkingUp", false);
        }

        if(inputVertical < 0 && inputHorizontal == 0){
            anim.SetBool("isWalkingDown", true);
        } else{
            anim.SetBool("isWalkingDown", false);
        }

    }

    void StartChasing()
    {
        isChasing = true;
        StartCoroutine(AStarPathfinding());
    }

    void StopChasing()
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
            if (distanceToNode < 0.1f)
            {
                // Remove the reached node from the path
                currentPath.RemoveAt(0);
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
        Vector3 movementDirection = (transform.position - previousPosition).normalized;

        previousPosition = transform.position;

        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;

        //FieldOfView.angle = angle;
        //FOVDetection.angle = angle;

        return movementDirection;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Coin") && coinToss.speed > 0f)
        {
            Debug.Log("Heard coin");
        }
    }
    
}