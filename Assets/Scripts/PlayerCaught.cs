using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCaught : MonoBehaviour
{
    private GameObject guard;

    void Start()
    {
        guard = GameObject.Find("Guard");
    }

    void Update()
    {
        if (guard != null)
        {
            float distance = Vector2.Distance(transform.position, guard.transform.position);

            if (distance < 0.5f)
            {
                EndGame();
            }
        }
    }

    void EndGame()
    {
         if (guard != null)
        {
            float distance = Vector2.Distance(transform.position, guard.transform.position);

            if (distance < 0.5f)
            {
                Debug.Log("Game Over");
            }
        }
    }
}
