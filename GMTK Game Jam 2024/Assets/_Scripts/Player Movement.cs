using System.Collections;
using System.Collections.Generic;
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
        Jumping();
        Grounded();
        Walking();
    }

    #region Ground Check

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
            hasJumped = false;
        }
        else if (isGrounded && !grounded)
        {
            isGrounded = false;
            timeLeftGround = Time.time;
        }
    }

    #endregion

    #region Walking

    [SerializeField] private float moveSpeed = 10; // Speed of regular walking
    [SerializeField] private float sprintSpeed = 13; // Speed of sprinting

    // Handle player walking
    private void Walking()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Adjust velocity based on sprint input
        if (Input.GetButton("Sprint"))
        {
            rb.velocity = new Vector2(horizontal * sprintSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

    }
    #endregion

    #region Jumping

    private float elapsedTimeSinceLeftGround; // Time elapsed since the player left the ground
    private float timeLeftGround; // Time when the player left the ground
    [SerializeField] private float coyoteTime = 0.2f; // Duration of grace period for jumping after leaving ground
    [SerializeField] private float jumpHeight = 12; // Height of the jump
    [SerializeField] private float fallSpeed = 7; // Speed of falling
    [SerializeField] private float jumpVelocityFalloff = 8; // Rate of decrease in jump velocity
    private bool hasJumped; // Flag indicating if the player has initiated a jump

    // Handle player jumping
    private void Jumping()
    {
        elapsedTimeSinceLeftGround = Time.time - timeLeftGround;
        if (Input.GetButtonDown("Jump") && (isGrounded || elapsedTimeSinceLeftGround < coyoteTime) && !hasJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            hasJumped = true;
        }

        // Apply gravity and falloff to jump velocity
        if (rb.velocity.y < jumpVelocityFalloff || rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * (fallSpeed * Physics.gravity.y * Time.deltaTime);
        }

    }
    #endregion
}
