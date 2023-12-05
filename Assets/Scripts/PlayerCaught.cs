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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == guard)
        {
            //transition to game over screen
            Debug.Log("GameOver");
        }
    }
}
