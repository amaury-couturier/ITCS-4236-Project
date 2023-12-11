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
    private DoublePillarController doublePillarController;
    
    void Start()
    {   
        currentNumberOfPaintings = 0;
        paintingsText.text = "Paintings: " + currentNumberOfPaintings + "/" + totalNumberOfPaitings;
        interactable = FindObjectOfType<Interactable>();
        pillarController = FindObjectOfType<PillarController>();
        doublePillarController = FindObjectOfType<DoublePillarController>();
    }

    
    void Update()
    {
        paintingsText.text = "Paintings: " + currentNumberOfPaintings + "/" + totalNumberOfPaitings;
    }
}