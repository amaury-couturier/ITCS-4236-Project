using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePillarController : MonoBehaviour
{
   public bool frontIsTaken;
   public bool backIsTaken;
   public bool bothTaken;
   [SerializeField] private Animator animator;
   
   public void TakePaintingDoublePillar()
   {
        if (!backIsTaken)
        {
            backIsTaken = true;
            animator.SetBool("BackIsTaken", backIsTaken);
        }
        if (!frontIsTaken)
        {
            frontIsTaken = true;
            animator.SetBool("FrontIsTaken", frontIsTaken);
        }
        if (frontIsTaken && backIsTaken && !bothTaken)
        {
            bothTaken = true;
            animator.SetBool("BothTaken", bothTaken);
        }
   }
}
