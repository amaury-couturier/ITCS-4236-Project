using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinToss : Interactable
{
    [SerializeField] private GameObject coin;
    private GameObject instantiatedCoin; // Store a reference to the instantiated coin.

    private bool tossed = false;

    void Update()
    {
        if (Input.GetKeyDown(interactKey) && !tossed)
        {
            tossed = true;
            instantiatedCoin = Instantiate(coin, transform.position, Quaternion.identity);
        }
        if (inRange && Input.GetKeyDown(interactKey) && tossed)
        {
            if (instantiatedCoin != null)
            {
                Destroy(instantiatedCoin); // Destroy the instantiated coin.
            }
        }
    }
}
