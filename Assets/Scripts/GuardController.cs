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
    [SerializeField] private Transform prefabFOV;
    [SerializeField] private float fovAngle;
    [SerializeField] private float viewDistance;
    private FieldOfView FOV;
    private Vector3 previousPosition;

    [Header("A* Chasing")]
    private GridController gridController;
    private bool isChasing = false;
    private List<Tile> currentPath;
    
    [Header("Patrolling")]
    [SerializeField] private Transform[] patrolPoints;
    private int targetPoint;

    void Start()
    {
        //previousPosition = transform.position;

        gridController = FindObjectOfType<GridController>();

        targetPoint = 0;

        if (gridController == null)
        {
            Debug.LogError("GridController not found.");
        }

        FOV = Instantiate(prefabFOV, null).GetComponent<FieldOfView>();
        FOV.SetFoV(fovAngle);
        FOV.SetViewDistance(viewDistance);
    }

    private void Update()
    {
        Patrol();

        SetAnimBools(rb.velocity.x, rb.velocity.y);

        //CHANGE ANGLE FOR EACH DIRECTION THE GUARD IS WALKING
        //SOMETHING OF THE SORT if (inputHorizontal < 0f) FOV.angle = ...
        FOV.SetOrigin(transform.position);
        // FOV.SetAimDirection(transform.position);
        FOV.SetAimDirection(GetAimDirection());

        FindTarget();
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

    /*public Vector3 GetAimDirection()
    {
        // Get the current position of the GameObject
        Vector3 currentPosition = transform.position;

        // Calculate the movement direction
        Vector3 movementDirection = currentPosition - previousPosition;

        // Check the movement direction
        if (movementDirection.magnitude > 0.001f) 
        {
            if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
            {
                if (movementDirection.x > 0)
                {
                    FOV.angle = FOV.startingAngle - FOV.fov;
                }
                else
                {
                    FOV.angle = FOV.startingAngle - FOV.fov;
                }
            }
            else
            {
                if (movementDirection.y > 0)
                {
                    FOV.angle = FOV.startingAngle - FOV.fov;
                }
                else
                {
                    FOV.angle = FOV.startingAngle - FOV.fov;
                }
            }
        }

        // Update the previous position for the next frame
        previousPosition = currentPosition;
        return currentPosition;
    }*/

    public Vector3 GetAimDirection()
    {
        // Calculate the movement direction
        Vector3 movementDirection = (transform.position - previousPosition).normalized;

        // Update the previous position for the next frame
        previousPosition = transform.position;

        // Calculate the angle from the movement direction
        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;

        // Adjust FOV angle based on the movement direction
        FOV.angle = angle;

        return movementDirection;
    }

    private void FindTarget()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < viewDistance)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            if (Vector3.Angle(transform.position, directionToPlayer) < fovAngle / 2f)
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance);
                if (raycastHit2D.collider != null)
                {
                    if (!raycastHit2D.collider.CompareTag("Player"))
                    {
                        //This if statement isn't ever true? I'm not too sure why
                        Debug.Log("Player detected"); 
                        //ADD LOGIC HERE
                        //This should set the guard's A* target to the player's current location,
                        //THe player's current location will be constantly so long as he is in the flashlight
                        //If he is not in the flashlight, the guard should move to the last seen location of the player
                    }
                    else
                    {
                        //This will trigger even if the the player is not currently in the actual flashlight
                        //Almost like there's a circle around the guard that triggers it
                        //I think this is due to the poor implementation of the GetAimDirection function
                        //Once GetAimDirection() is properly implemented, this should work correctly I believe
                        Debug.Log("Player not detected");
                    }
                }
            }
        }
    }
}