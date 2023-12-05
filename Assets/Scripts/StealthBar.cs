using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StealthBar : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Image fillImage;
    private Slider slider;
    [SerializeField] private float fillValue = 0.02f;
    private float fillTimer = 0.0f;
    [SerializeField] private float fillInterval = 0.25f;

    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        slider = GetComponent<Slider>();
        slider.value = 0.0f;
    }

    void Update()
    {
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }
        if (slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        if ((Mathf.Abs(playerController.inputHorizontal) != 0.0f) || (Mathf.Abs(playerController.inputVertical) != 0.0f))
        {
            // Accumulate time
            fillTimer += Time.deltaTime;

            // Check if the fill interval is reached
            if (fillTimer >= fillInterval)
            {
                slider.value += fillValue;
                fillTimer = 0.0f; // Reset the timer
            }
        }
    }
}
