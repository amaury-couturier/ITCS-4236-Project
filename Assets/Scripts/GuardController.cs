using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform guardTransform;
    [SerializeField] private float detectionThreshold = 0.001f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent MeshAgent;

    private GridController gridController;
    private bool isChasing = false;
    private List<Tile> currentPath;

    void Start()
    {
        gridController = FindObjectOfType<GridController>();

        if (gridController == null)
        {
            Debug.LogError("GridController not found.");
        }
    }

    private void Update()
    {
         // Get the vector from the guard to the player
        Vector2 toPlayer = playerTransform.position - guardTransform.position;

        // Get the forward direction of the guard
        Vector2 guardForward = guardTransform.up;

        // Calculate the dot product
        float dotProduct = Vector2.Dot(toPlayer.normalized, guardForward);

        if (dotProduct > detectionThreshold)
        {
            // Implement A* pathfinding algorithm here
            Debug.Log("Player is in front of the guard.");

            if (!isChasing)
            {
                StartChasing();
            }

        } 
        else 
        {
            // Implement a "StopChasing()" function here
            Debug.Log("Player is out of sight.");

            if (isChasing)
            {
                StopChasing();
            }
        }

        SetAnimBools(rb.velocity.x, rb.velocity.y);

    } 

    void SetAnimBools(float inputHorizontal, float inputVertical)
    {
        if(inputHorizontal != 0){
            anim.SetBool("isWalkingRight", true);
        } else{
            anim.SetBool("isWalkingRight", false);
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
}