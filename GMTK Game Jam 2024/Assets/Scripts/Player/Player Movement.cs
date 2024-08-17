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
        animator = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    private void Update()
    {
        // Handle player movement functions
        Grounded();
        Animations();
        if(isDashing) return;
        WallJumping();
        if(isWallJumping) return;
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        if (isGrounded)
        {
            isGrounded = true;
            jumpsRemaining = numberOfJumps;
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
            FlipSprite();
        }
        else if (horizontal < 0 && facingRight)
        {
            FlipSprite();
        }
    }

    private void FlipSprite(){
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    #endregion

    #region Jumping



    [Header("Jump Settings")]
    [SerializeField] private bool playerCanDoubleJump = true; // Flag indicating if the player can jump
    [SerializeField] private float coyoteTime = 0.2f; // Duration of grace period for jumping after leaving ground
    [SerializeField] private float jumpBufferTime = 0.1f; // Duration of buffer time for jumping
    [SerializeField] private float jumpSpeed = 12; // Height of the jump
    [SerializeField] private float fallSpeed = 7; // Speed of falling
    [SerializeField] private float jumpVelocityFalloff = 8; // Rate of decrease in jump velocity
    
    private int numberOfJumps = 1; // Number of jumps the player can perform
    private int jumpsRemaining; // Number of jumps remaining
    private float jumpBufferCounter; // Counter for jump buffer time
    private float coyoteTimeCounter; // Counter for coyote time

    private bool hasJumped; // Flag indicating if the player has initiated a jump for animations
    // Handle player jumping
    private void Jumping()
    {
        // Reset coyote time counter if the player is grounded
        if(isGrounded){
            coyoteTimeCounter = coyoteTime;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump")){
            jumpBufferCounter = jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        // Coyote time jump (first jump off the ground)
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {       
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            ChangeAnimationState(PLAYER_JUMP_START);
        }

        if(Input.GetButtonUp("Jump")){
            coyoteTimeCounter = 0; // Reset coyote time counter if the player releases the jump button to stop accidental double jumps
        }

        // Apply gravity and falloff to jump velocity
        if (rb.velocity.y < jumpVelocityFalloff || rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * (fallSpeed * Physics.gravity.y * Time.deltaTime);
            ChangeAnimationState(PLAYER_FALL);
        }


        if(!playerCanDoubleJump) return;
 
        // Mid air jumps - if the player is not grounded and has jumps remaining they can jump again
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter < 0 && jumpsRemaining > 0 && !isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            jumpsRemaining--;
        }
        
    }
    #endregion

    #region Wall Jumping
    [Header("General Wall Settings")]
    [SerializeField] private bool playerCanWallJump = true; // Flag indicating if the player can wall jump
    [SerializeField] private LayerMask wallMask; // The layer mask for wall objects
    [SerializeField] private Transform wallCheck; // The transform representing the position to check for walls
    private bool isTouchingWall; // Flag indicating if the player is touching a wall

    [Header("Wall Slide Settings")]
    [SerializeField] private float wallSlideSpeed = 2f; // Speed of sliding down a wall
    private bool isWallSliding; // Flag indicating if the player is sliding down a wall

    [Header("Wall Jump Settings")]
    [SerializeField] private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingDuration = 0.2f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f,16f);
    private bool isWallJumping; // Flag indicating if the player is wall jumping
    private float wallJumpingDir;
    private float wallJumpingCounter;

    private void WallJumping(){
        if(!playerCanWallJump) return;

        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, wallMask);


        WallSlide();

        WallJump();
    }

    private void WallSlide(){
        // If the player is touching a wall and not grounded and moving horizontally into the wall, slide down the wall
        if(isTouchingWall && !isGrounded && horizontal != 0){
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            isWallSliding = true;
        } else {
            isWallSliding = false;
        }
    }

    private void WallJump(){
        if(isWallSliding){
            isWallJumping = false;

            wallJumpingDir = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime; // gives a small buffer time to jump off the wall

            CancelInvoke("StopWallJumping");
        } else {
            wallJumpingCounter -= Time.deltaTime; 
        }

        if(Input.GetButtonDown("Jump") && wallJumpingCounter > 0){
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0;
            if(transform.localScale.x != wallJumpingDir){
                FlipSprite();
            }

            Invoke("StopWallJumping", wallJumpingDuration);
        }
    }

    private void StopWallJumping(){
        isWallJumping = false;
    }
    #endregion

    #region Dashing
    private bool canDash = true; // Flag indicating if the player can dash
    private bool isDashing = false; // Flag indicating if the player is in the middle of dashing

    [Header("Dash Settings")]
    [SerializeField] private bool playerCanDash = true; // Flag indicating if the player can dash
    [SerializeField] private float dashPower = 24f; // Power/speed of the dash
    [SerializeField] private float dashDuration = 0.2f; // How long the dash lasts
    [SerializeField] private float dashCooldown = 1f; // Cooldown time between dashes


    private void Dashing(){
        if(!playerCanDash) return;
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
        canDash = true; // Allow the player to dash again
    }
    #endregion

    #region Animation

    //animation states    
    const string PLAYER_WALK = "Walk";
    const string PLAYER_RUN = "Run";
    const string PLAYER_IDLE = "Still";
    const string PLAYER_JUMP_START = "AirUp";
    const string PLAYER_RISE = "AirMid";
    const string PLAYER_FALL = "AirDown";
    const string PLAYER_DASH = "Dash";


    private Animator animator;
    private string currentState;
    

    private void Animations(){
        if(isDashing){
            ChangeAnimationState(PLAYER_DASH);
            return;
        }
        
        if(rb.velocity.x != 0){
             if(Input.GetButton("Sprint")){
                ChangeAnimationState(PLAYER_RUN);
            } else {
                ChangeAnimationState(PLAYER_WALK);
            }
        } else{
            ChangeAnimationState(PLAYER_IDLE);
        }
    }
    public void JumpAnimComplete()
    {
        ChangeAnimationState(PLAYER_RISE);
    }

    void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if(currentState == newState) return;
        
        //play the animation
        animator.Play(newState);
        
        //reassign the current state
        currentState = newState;
    }

    #endregion
}
