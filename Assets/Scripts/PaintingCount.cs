using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PaintingCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI paintingsText;
    [SerializeField] private int totalNumberOfPaitings;
    public int currentNumberOfPaintings;
    private Interactable interactable;
    private PillarController pillarController;
    [SerializeField] private GameObject blockedDoor;

    void Start()
    {   
        currentNumberOfPaintings = 0;
        paintingsText.text = "Paintings: " + currentNumberOfPaintings + "/" + totalNumberOfPaitings;
        interactable = FindObjectOfType<Interactable>();
        pillarController = FindObjectOfType<PillarController>();
    }

    
    void Update()
    {
        paintingsText.text = "Paintings: " + currentNumberOfPaintings + "/" + totalNumberOfPaitings;

        if (currentNumberOfPaintings >= totalNumberOfPaitings)
        {
            currentNumberOfPaintings = totalNumberOfPaitings;
            if (blockedDoor != null)
            {
                blockedDoor.GetComponent<Collider2D>().isTrigger = true;
            }
        }
    }
}
