using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour
{
   public bool isTaken;
   [SerializeField] private Animator animator;
   
   public void TakePainting()
   {
        if (!isTaken)
        {
            isTaken = true;
            animator.SetBool("IsTaken", isTaken);
        }
   }
}
