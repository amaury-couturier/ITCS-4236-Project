<<<<<<< Updated upstream
using System.Collections;
using System.Collections.Generic;
=======
>>>>>>> Stashed changes
using UnityEngine;

public class GuardController : MonoBehaviour
{
<<<<<<< Updated upstream
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

        /*if (dotProduct > detectionThreshold)
        {
            // Implement A* pathfinding algorithm here
            Debug.Log("Player is in front of the guard.");
        }
        else
        {
            // Implement a "StopChasing()" function here
            Debug.Log("Player is behind the guard.");
        }*/
    } 
=======
    [SerializeField] private Collider2D flashlight;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Detects Player!");
        }
    }
>>>>>>> Stashed changes
}
