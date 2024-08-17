using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    

    // Start is called before the first frame update
    private void Awake()
    {
        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Handle player movement functions
        Grounded();
        if(isDashing) return;
        Jumping();
        Walking();
        Dashing();
    }

    #region Ground Check

    [Header ("Ground Check")]
    [SerializeField] private LayerMask groundMask; // The layer mask for ground objects
    [SerializeField] private Transform groundCheck; // The transform representing the position to check for ground
    [SerializeField] private float groundCheckRadius = 0.2f; // The radius for ground check
    private bool isGrounded; // Flag indicating if the player is grounded
    private readonly Collider2D[] ground = new Collider2D[1]; // Array to store colliders of objects on ground

    // Check if the player is grounded
    private void Grounded()
    {
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        if (!isGrounded && grounded)
        {
            isGrounded = true;
            jumpsRemaining = MidAirJumps;
            hasJumped = false;
            canDash = true;
        }
        else if (isGrounded && !grounded)
        {
            isGrounded = false;
            timeLeftGround = Time.time;
        }
    }

    #endregion

    #region Walking

    [Header("Movement Speeds")]
    [SerializeField] private float moveSpeed = 10; // Speed of regular walking
    [SerializeField] private float sprintSpeed = 13; // Speed of sprinting
    private float horizontal; // Horizontal input from player
    
    private bool facingRight = true; // Flag indicating if the player is facing right
    // Handle player walking
    private void Walking()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Adjust velocity based on sprint input
        if (Input.GetButton("Sprint"))
        {
            rb.velocity = new Vector2(horizontal * sprintSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        // Flip the player sprite if necessary
        if (horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip();
        }
    }

    private void Flip(){
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    #endregion

    #region Jumping

    private float elapsedTimeSinceLeftGround; // Time elapsed since the player left the ground
    private float timeLeftGround; // Time when the player left the ground

    [Header("Jump Settings")]
    [SerializeField] private float coyoteTime = 0.2f; // Duration of grace period for jumping after leaving ground
    [SerializeField] private float jumpHeight = 12; // Height of the jump
    [SerializeField] private float fullJumpDuration = 0.2f; // Duration of the jump
    [SerializeField] private float fallSpeed = 7; // Speed of falling
    [SerializeField] private float jumpVelocityFalloff = 8; // Rate of decrease in jump velocity
    [SerializeField] private int MidAirJumps = 1; // Number of jumps the player can perform

    private int jumpsRemaining; // Number of jumps remaining
    private bool hasJumped; // Flag indicating if the player has initiated a jump
    // Handle player jumping
    private void Jumping()
    {
        elapsedTimeSinceLeftGround = Time.time - timeLeftGround;
  
        // Coyote time jump (first jump off the ground)
        if (Input.GetButtonDown("Jump") && (isGrounded || elapsedTimeSinceLeftGround < coyoteTime) && !hasJumped)
        {       
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            hasJumped = true;
        }


        // Mid air jumps - if the player is not grounded and has jumps remaining they can jump again
        if (Input.GetButtonDown("Jump") && !isGrounded && jumpsRemaining > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpsRemaining--;
        }

        // Apply gravity and falloff to jump velocity
        if (rb.velocity.y < jumpVelocityFalloff || rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * (fallSpeed * Physics.gravity.y * Time.deltaTime);
        }

    }
    #endregion

    #region Dashing
    private bool canDash = true; // Flag indicating if the player can dash
    private bool isDashing = false; // Flag indicating if the player is in the middle of dashing

    [Header("Dash Settings")]
    [SerializeField] private float dashPower = 24f; // Power/speed of the dash
    [SerializeField] private float dashDuration = 0.2f; // How long the dash lasts
    [SerializeField] private float dashCooldown = 1f; // Cooldown time between dashes


    private void Dashing(){

        // If the player presses the dash button and can dash, start the dash coroutine
        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    
    private IEnumerator Dash()
    {
        canDash = false; // Prevent the player from dashing again until the cooldown is over - is set to true in the grounded function
        isDashing = true; // Sets flag that the player is currently dashing
        float originalGravity = rb.gravityScale; // Store the original gravity scale of the player
        rb.gravityScale = 0; // Set gravity to 0 so the player doesn't fall during the dash
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f); // Apply the dash power to the player

        yield return new WaitForSeconds(dashDuration); // Wait for the dash duration to end

        rb.gravityScale = originalGravity; // Reset the gravity scale
        isDashing = false; // Reset the dashing flag

        yield return new WaitForSeconds(dashCooldown); // Wait for the dash cooldown to end

    }
    #endregion
}
