using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StealthBar : MonoBehaviour
{
    private PlayerController playerController;
    private GuardController guardController;
    [SerializeField] private Image fillImage;
    private Slider slider;
    [SerializeField] private float fillValue = 0.02f;
    [SerializeField] private float decreaseValue = 0.04f;
    [SerializeField] private float dashValue = 0.05f;
    private float fillTimer = 0.0f;
    [SerializeField] private float fillInterval = 0.25f;
    public AudioClip audioClipThreshold;
    public GameObject player;
    public GameObject guard;

    void Awake()
    {
        player = GameObject.Find("Player");
        guard = GameObject.Find("Guard");
        playerController = player.GetComponent<PlayerController>();
        guardController = guard.GetComponent<GuardController>();
        slider = GetComponent<Slider>();
        slider.value = 0.0f;
    }

    void Update()
    {
        if (guardController != null)
        {
            if (slider.value <= slider.minValue)
            {
                fillImage.enabled = false;
            }
            if (slider.value > slider.minValue && !fillImage.enabled)
            {
                fillImage.enabled = true;
            }
            if (slider.value >= 1f)
            {
                guardController.StartChasing();
                AudioSource.PlayClipAtPoint(audioClipThreshold, player.transform.position, 0.5f);
            }
        }

        if (playerController != null)
        {
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
            if (playerController.inputHorizontal == 0.0f && playerController.inputVertical == 0.0f)
            {
                fillTimer += Time.deltaTime;

                // Check if the fill interval is reached
                if (fillTimer >= fillInterval)
                {
                    slider.value -= decreaseValue;
                    fillTimer = 0.0f; // Reset the timer
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                slider.value += dashValue;
            }
        }
    }
}
