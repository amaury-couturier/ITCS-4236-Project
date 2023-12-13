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
    private bool hasInteracted = false;

    [SerializeField] private float interactableRadius = 0.1f;

    void Start()
    {
        paintingCount = FindObjectOfType<PaintingCount>(); 
    }

    void Update()
    {
        if (inRange)
        {
            if (Input.GetKeyDown(interactKey) && pillarController != null && !pillarController.isTaken && !hasInteracted)
            {
                interactionAction.Invoke();
                paintingCount.currentNumberOfPaintings++;
                hasInteracted = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;

            LayerMask interactableLayerMask = LayerMask.GetMask("Unwalkable");

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactableRadius, interactableLayerMask);

            foreach (Collider2D col in colliders)
            {
                PillarController foundController = col.GetComponent<PillarController>();
                if (foundController != null)
                {
                    pillarController = foundController;
                    break;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
            pillarController = null;
        }
    }
}
