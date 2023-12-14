 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour
{
   public bool isTaken;
   [SerializeField] private Animator animator;
   public AudioClip audioClipRipping;
   public GameObject player;
   
   public void TakePainting()
   {
        if (!isTaken)
        {
            isTaken = true;
            AudioSource.PlayClipAtPoint(audioClipRipping, player.transform.position, 0.5f);
            animator.SetBool("IsTaken", isTaken);
        }
   }
}
