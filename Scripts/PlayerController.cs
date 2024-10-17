using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 20f;
    public float wallSlideSpeed = 2f;
    public float doubleJumpForce = 8f;
    public float wallJumpForceX = 8f;  // Forza laterale per il side jump

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isRoofed;

    private bool canDoubleJump;
    private bool isCrouching;
    private bool isWallSliding;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    public Transform roofCheck;
    public LayerMask roofLayer;
    public float roofCheckRadius = 0.2f;
    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    public LayerMask wallLayer;
    public float wallCheckDistance = 0.5f;

    private bool isTouchingRightWall;
    private bool isTouchingLeftWall;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    float lastMove = 1;
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (!isWallSliding && moveInput!=0)
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            if (moveInput > 0)
            {
                lastMove = 1;
             }
             else {if(moveInput < 0)
                    lastMove = -1;
                }
            Debug.Log(moveInput);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
                canDoubleJump = true;
            }
            else if (isWallSliding)
            {
                SideJump();
            }else if (canDoubleJump)
            {
                DoubleJump();
                canDoubleJump = false;
            }
             
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
            
        }

        if (Input.GetKeyDown(KeyCode.C))
        {   if(!isCrouching)
                Crouch();
            else
            {
                isRoofed = Physics2D.OverlapCircle(roofCheck.position, roofCheckRadius, roofLayer);
                if (!isRoofed)
                    StandUp();
            }
        }
        //else if (Input.GetKeyUp(KeyCode.C))
        //{
            
        //}

        isTouchingRightWall = Physics2D.Raycast(wallCheckRight.position, Vector2.right, wallCheckDistance, wallLayer);
        isTouchingLeftWall = Physics2D.Raycast(wallCheckLeft.position, Vector2.left, wallCheckDistance, wallLayer);

        if ((isTouchingRightWall || isTouchingLeftWall) && !isGrounded)
        {
            StartWallSliding();
        }
        else
        {
            StopWallSliding();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void SideJump()
    {
        if (isTouchingRightWall)
        {
            // Salta verso sinistra se tocca il muro a destra
            rb.velocity = new Vector2(-wallJumpForceX, jumpForce);
        }
        else if (isTouchingLeftWall)
        {
            // Salta verso destra se tocca il muro a sinistra
            rb.velocity = new Vector2(wallJumpForceX, jumpForce);
        }

        //StopWallSliding(); // Ferma il wall sliding dopo il sidejump
    }

    void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
    }

    void Dash()
    {
        Debug.Log("dash");
        Debug.Log(lastMove);
        rb.velocity = new Vector2(lastMove*transform.localScale.x * dashForce, rb.velocity.y);
        
    }

    void Crouch()
    {
        isCrouching = true;
        transform.localScale = new Vector3(transform.localScale.x, 0.5f, transform.localScale.z);
    }

    void StandUp()
    {
        isCrouching = false;
        transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
    }

    void StartWallSliding()
    {
        isWallSliding = true;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
    }

    void StopWallSliding()
    {
        isWallSliding = false;
    }
}
