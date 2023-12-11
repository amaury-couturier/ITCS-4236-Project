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
    private DoublePillarController doublePillarController;

    void Start()
    {
        paintingCount = FindObjectOfType<PaintingCount>(); 
        pillarController = FindObjectOfType<PillarController>();
        doublePillarController = FindObjectOfType<DoublePillarController>();
    }

    void Update()
    {
        if (inRange)
        {
            // Doesn't work since you can just spam when in range of the interactable object and it iwll augment
            // the double pilalr is causing lots of issues since I can't jsut use one variable for all the pillars now
            // The logic is here but yeah, it makes sense as to why it doesn't work
            if ((Input.GetKeyDown(interactKey) && !pillarController.isTaken))
            {
                paintingCount.currentNumberOfPaintings++;
                interactionAction.Invoke();
            }
            else if (Input.GetKeyDown(interactKey) && !doublePillarController.frontIsTaken)
            {
                paintingCount.currentNumberOfPaintings++;
                interactionAction.Invoke();
            }
            else if (Input.GetKeyDown(interactKey) && !doublePillarController.backIsTaken)
            {
                paintingCount.currentNumberOfPaintings++;
                interactionAction.Invoke();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
