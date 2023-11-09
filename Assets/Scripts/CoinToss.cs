using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinToss : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] public KeyCode interactKey;
    public GameObject instantiatedCoin;
    public bool tossed = false;

    void Update()
    {
        if (Input.GetKeyDown(interactKey) && !tossed)
        {
            tossed = true;
            instantiatedCoin = Instantiate(coin, transform.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") /*&& tossed*/)
        {
            Debug.Log("ENtered");
            Destroy(instantiatedCoin);
            tossed = false;
        }
    }
}
