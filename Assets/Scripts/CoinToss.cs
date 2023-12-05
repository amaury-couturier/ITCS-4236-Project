using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinToss : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private KeyCode interactKey;
    [SerializeField] private float speed = 10.0f;
    private GameObject instantiatedCoin;
    private Vector2 targetPosition;
    private bool tossed = false;

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition = mousePosition;

        // Check if there is no instantiatedCoin and the interactKey is pressed
        if (instantiatedCoin == null && Input.GetKeyDown(interactKey) && !tossed)
        {
            instantiatedCoin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            tossed = true;
        }

        if (instantiatedCoin != null && speed > 0)
        {
            speed -= Random.Range(.1f, .25f);
            instantiatedCoin.transform.position = Vector2.MoveTowards(instantiatedCoin.transform.position, targetPosition, speed * Time.deltaTime);
        }
        else if (speed <= 0)
        {
            speed = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Coin") && speed <= 0f)
        {
            Destroy(instantiatedCoin);
            speed = 10.0f;
            tossed = false;
        }
    }
}
