using System.Collections;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    [SerializeField] private GuardController guardController;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            //Vector3 lookDirection;
            Vector3 moveDirection = guardController.GetAimDirection();
            

            if (Vector2.Angle(moveDirection, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask);
                
                if (hit.collider == null)
                {
                    canSeePlayer = true;
                    Debug.Log("can see player");
                    guardController.StartChasing();
                }
                else
                {
                    canSeePlayer = false;
                    guardController.StopChasing();
                }
            }
            else
            {
                canSeePlayer = false;
                guardController.StopChasing();
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
            guardController.StopChasing();
        }
    }
}
