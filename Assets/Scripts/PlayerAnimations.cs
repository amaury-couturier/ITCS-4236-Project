using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerController playerController;

    public string currentState;
    public string playerIdle = "Idle";
    public string walkRight = "Walk_Right";
    public string walkUp = "Character_Walk_Up";
    public string walkDown = "Character_Walk_Down";

    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerController.isWalking && !playerController.isDashing)
        {
            PlayAnimation(playerIdle);
        }
        else if (Input.GetKeyDown(KeyCode.D) && playerController.isWalking && playerController.isFacingRight && !playerController.isDashing)
        {
            PlayAnimation(walkRight);
        }
        else if (Input.GetKeyDown(KeyCode.W) && playerController.isWalking && !playerController.isDashing)
        {
            PlayAnimation(walkUp);
        }
        else if (Input.GetKeyDown(KeyCode.S) && playerController.isWalking && !playerController.isDashing)
        {
            PlayAnimation(walkDown);
        }
    }

    public void PlayAnimation(string newState)
    {
        if (currentState == newState) return;
        playerAnimator.Play(newState);
        currentState = newState;
    }
}
