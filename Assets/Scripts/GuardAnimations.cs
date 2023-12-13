using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAnimations : MonoBehaviour
{
    private Animator guardAnimator;
    private GuardController guardController;

    public string currentState;
    public string guardIdle = "Guard_Idle";
    public string walkRight = "Guard_Walk_Right";
    public string walkUp = "Guard_Walk_Up";
    public string walkDown = "Guard_Walk_Down";

    void Start()
    {
        guardAnimator = GetComponentInChildren<Animator>();
        guardController = GetComponent<GuardController>();
    }

    void Update()
    {
        if (!guardController.isWalkingRight && !guardController.isWalkingLeft && !guardController.isWalkingUp && !guardController.isWalkingDown)
        {
            PlayAnimation(guardIdle);
        }
        else if (guardController.isWalkingRight && guardController.isFacingRight)
        {
            PlayAnimation(walkRight);
        }
        else if (guardController.isWalkingLeft && !guardController.isFacingRight)
        {
            PlayAnimation(walkRight);
        }
        else if (guardController.isWalkingUp)
        {
            PlayAnimation(walkUp);
        }
        else if (guardController.isWalkingDown)
        {
            PlayAnimation(walkDown);
        }
    }

    public void PlayAnimation(string newState)
    {
        if (currentState == newState) return;
        guardAnimator.Play(newState);
        currentState = newState;
    }
}
