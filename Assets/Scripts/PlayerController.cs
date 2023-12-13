using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float playerMaxSpeed;
    [SerializeField] private float acceleration;
    private float targetVelocityX;
    private float targetVelocityY;
    private float currentVelocityX;
    private float currentVelocityY;
    public float inputHorizontal;
    public float inputVertical;
    public bool isWalkingRight;
    public bool isWalkingLeft;
    public bool isWalkingUp;
    public bool isWalkingDown;
    
    [Header("Player Dash")]
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    private bool canDash = true;
    public bool isDashing;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;
    public bool isFacingRight = true;
    [SerializeField] private Animator anim;
    private GameObject guard;

    void Awake()
    {
        guard = GameObject.Find("Guard");
    }

    void Update()
    {
        //Simply return in case isDashing is true so the player is not allowed to move or jump while dahsing
        if(isDashing)
        {
            return;
        }
        
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        if (isDashing || (Mathf.Abs(inputHorizontal) == 0.0f && Mathf.Abs(inputVertical) == 0.0f))
        {
            isWalkingRight = false;
            isWalkingLeft = false;
            isWalkingUp = false;
            isWalkingDown = false;
        }
        else if (!isDashing && inputHorizontal > 0.0f)
        {
            isWalkingRight = inputHorizontal > 0.0f;
        }
        else if (!isDashing && inputHorizontal < 0.0f)
        {
            isWalkingLeft = inputHorizontal < 0.0f;
        }
        else if (!isDashing && inputVertical > 0.0f)
        {
            isWalkingUp = inputVertical > 0.0f;
        }
        else if (!isDashing && inputVertical < 0.0f)
        {
            isWalkingDown = inputVertical < 0.0f;
        }

        CheckDashInput();

        Flip();

        EndGame();
    }

    void FixedUpdate()
    {
        //Simply return in case isDashing is true so the player is not allowed to move or jump while dahsing
        if(isDashing)
        {
            return;
        }

        PlayerMovement();
    }

    public void PlayerMovement() 
    {
        float targetVelocityX = inputHorizontal * playerMaxSpeed;
        float targetVelocityY = inputVertical * playerMaxSpeed;

        // Apply acceleration/deceleration to movement
        targetVelocityX = inputHorizontal * playerMaxSpeed;
        float tx = acceleration * Time.deltaTime;
        currentVelocityX = Mathf.Lerp(currentVelocityX, targetVelocityX, tx);
        rb.velocity = new Vector2(currentVelocityX, rb.velocity.y);

        targetVelocityY = inputVertical * playerMaxSpeed;
        float ty = acceleration * Time.deltaTime;
        currentVelocityY = Mathf.Lerp(currentVelocityY, targetVelocityY, ty);
        rb.velocity = new Vector2(rb.velocity.x, currentVelocityY);
    }

    private void CheckDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void Flip()
    {
        if((isFacingRight && inputHorizontal < 0f) || (!isFacingRight && inputHorizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void EndGame()
    {
         if (guard != null)
        {
            float distance = Vector2.Distance(transform.position, guard.transform.position);

            if (distance < 0.5f)
            {
                Debug.Log("Game Over");
            }
        }
    }
}
