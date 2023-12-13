using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool inRange;
    public KeyCode interactKey; 
    public UnityEvent interactionAction;
    private PaintingCount paintingCount;
    private PillarController pillarController;

    void Start()
    {
        paintingCount = FindObjectOfType<PaintingCount>(); 
    }

    void Update()
    {
        if (inRange)
        {
            if (Input.GetKeyDown(interactKey) && pillarController != null && !pillarController.isTaken)
            {
                interactionAction.Invoke();
                paintingCount.currentNumberOfPaintings++;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
            pillarController = collision.gameObject.GetComponent<PillarController>();
            Debug.Log("Entered Range: " + pillarController.gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            pillarController = null;
            Debug.Log("Exited Range");
        }
    }
}
