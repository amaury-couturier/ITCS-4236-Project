using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinToss : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] public KeyCode interactKey;
    private GameObject instantiatedCoin; // Store a reference to the instantiated coin.
    [SerializeField] private float radiusOfSatisfaction = 1.5f;

    private bool tossed = false;

    void Update()
    {
        if (Input.GetKeyDown(interactKey) && !tossed)
        {
            tossed = true;
            instantiatedCoin = Instantiate(coin, transform.position, Quaternion.identity);
        }
        
        if (tossed)
        {
            float distance = Vector3.Distance(transform.position, instantiatedCoin.transform.position);

            if (instantiatedCoin != null && Input.GetKeyDown(interactKey) && distance <= radiusOfSatisfaction)
            { 
                tossed = false;
                Destroy(instantiatedCoin); // Destroy the instantiated coin.
            }
        }
    }
}
