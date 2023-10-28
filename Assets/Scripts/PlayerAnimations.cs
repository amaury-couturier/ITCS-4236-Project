using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerController playerController;

    public string currentState;
    public string playerIdle = "Character_Idle";
    public string walkRight = "Character_Walk_Right";
    public string walkUp = "Character_Walk_Up";
    public string walkDown = "Character_Walk_Down";

    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerController.isWalkingRight && !playerController.isWalkingLeft && !playerController.isWalkingUp && !playerController.isWalkingDown && !playerController.isDashing)
        {
            PlayAnimation(playerIdle);
        }
        else if (playerController.isWalkingRight && playerController.isFacingRight && !playerController.isDashing)
        {
            PlayAnimation(walkRight);
        }
        else if (playerController.isWalkingLeft && !playerController.isFacingRight && !playerController.isDashing)
        {
            PlayAnimation(walkRight);
        }
        else if (playerController.isWalkingUp && !playerController.isDashing)
        {
            PlayAnimation(walkUp);
        }
        else if (playerController.isWalkingDown && !playerController.isDashing)
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
