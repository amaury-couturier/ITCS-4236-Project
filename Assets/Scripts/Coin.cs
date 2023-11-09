using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Vector2 targetPosition;
    [SerializeField] private float speed = 5.0f;
    private CoinToss coinToss;

    void Start()
    {
        coinToss = GetComponent<CoinToss>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition = mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (speed > 0)
        {
            speed -= Random.Range(.1f, .25f);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime); 
        }
        else if (speed <= 0)
        {
            speed = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && coinToss.tossed)
        {
            Destroy(coinToss.instantiatedCoin);
            coinToss.tossed = false;
        }
    }
}
