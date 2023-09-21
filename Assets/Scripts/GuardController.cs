using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform guardTransform;
    [SerializeField] private float detectionThreshold = 0.4f;

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
        }
        else
        {
            // Implement a "StopChasing()" function here
            Debug.Log("Player is behind the guard.");
        }
    } 
}
