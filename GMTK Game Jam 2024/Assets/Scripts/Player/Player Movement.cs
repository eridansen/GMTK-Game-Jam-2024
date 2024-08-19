using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerCombat playerCombat;
    private TrailRenderer trailRenderer;


    // Start is called before the first frame update
    private void Awake()
    {
        // Get the components attached to the GameObject
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }
    private void Start() {
        LoadSettings();
    }
    // Update is called once per frame
    private void Update()
    {
        // Handle player movement functions 
        Grounded();
        Animations();

        if (isDashing || isKnockbacked) return;
        if (uninterruptibleAnim || isDead || isHurt)
        {
            rb.velocity = rb.velocity += Vector2.up * (_fallSpeed * Physics.gravity.y * Time.deltaTime);
            if (isDashing) rb.velocity -= new Vector2(rb.velocity.x / 2, 0);
            if (isGrounded) rb.velocity = Vector2.zero;
            return;
        }
        WallJumping();
        if (isWallJumping) return;
        Scaling();
        Jumping();
        Walking();
        Dashing();
    }


    bool isKnockbacked = false;

    public IEnumerator Knockback(Vector3 direction, Vector2 force, float duration)
    {  
        isKnockbacked = true;
        float timer = 0;
        while (duration > timer)
        {
            timer += Time.deltaTime;
            rb.velocity = new Vector2(-transform.localScale.x * force.x, force.y);
            yield return null;
        }
        isKnockbacked = false;

    }


    #region Player Scale Settings

    [Header("Player Scale Settings")]
    [SerializeField] private PlayerScaleSettings _currentPlayerScaleSettings;
    [SerializeField] private PlayerScaleSettings[] _arrayOfPlayerScaleSettings;
    [SerializeField] private float scaleChangeSpeed = 0.1f;
    [SerializeField] private float scaleChangeAmount = 0.1f;

    private int currentScale = 1;
    private bool isScaling = false;

    private void Scaling()
    {
        if(isScaling) return;
        
        if (Input.GetButtonDown("Scale Up"))
        {
            if (currentScale < _arrayOfPlayerScaleSettings.Length - 1)
            {
                currentScale++;
            }
        }
        if (Input.GetButtonDown("Scale Down"))
        {
            if (currentScale > 0)
            {
                currentScale--;
            }
        }
        if (_currentPlayerScaleSettings != _arrayOfPlayerScaleSettings[currentScale])
        {
            _currentPlayerScaleSettings = _arrayOfPlayerScaleSettings[currentScale];
            LoadSettings();
        }
    }

    private void ChangeSize()
    {
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector2(_currentPlayerScaleSettings.playerScale.x, _currentPlayerScaleSettings.playerScale.y);
        }
        else
        {
            transform.localScale = new Vector2(-_currentPlayerScaleSettings.playerScale.x, _currentPlayerScaleSettings.playerScale.y);
        }
    }


    public IEnumerator ScaleOverTime()
    {

        float initYValue = transform.localScale.y;
        float targetYValue = _currentPlayerScaleSettings.playerScale.y;

        float initXValue = transform.localScale.x;
        float targetXValue = _currentPlayerScaleSettings.playerScale.x;

        if (transform.localScale.x < 0)
        {
            targetXValue = -targetXValue; // if the x value is negative then the player is facing left, so the target x value needs to also be negative
        }

        float iterator = 0;
        while (iterator < 1)
        {
            isScaling = true;
            Vector2 newScale = new Vector2(Mathf.Lerp(initXValue, targetXValue, iterator), Mathf.Lerp(initYValue, targetYValue, iterator));
            transform.localScale = newScale;
            iterator += scaleChangeAmount;
            yield return new WaitForSeconds(scaleChangeSpeed);
        }

        ChangeSize();
        isScaling = false;

    }



    private void LoadSettings()
    {
        //size settings
        StartCoroutine(ScaleOverTime());
        rb.gravityScale = _currentPlayerScaleSettings.gravityScale;

        //movement settings
        _moveSpeed = _currentPlayerScaleSettings.walkSpeed;
        _sprintSpeed = _currentPlayerScaleSettings._sprintSpeed;

        //jump settings
        _jumpSpeed = _currentPlayerScaleSettings.jumpSpeed;
        _fallSpeed = _currentPlayerScaleSettings.fallSpeed;
        _jumpVelocityFalloff = _currentPlayerScaleSettings._jumpVelocityFalloff;

        //wall jump settings
        _wallSlideSpeed = _currentPlayerScaleSettings.wallSlideSpeed;
        _wallJumpingPower = _currentPlayerScaleSettings.wallJumpingPower;

        //dash settings
        _dashPower = _currentPlayerScaleSettings.dashPower;
        _dashDuration = _currentPlayerScaleSettings.dashDuration;
        _dashCooldown = _currentPlayerScaleSettings.dashCooldown;
    }
    #endregion

    #region Ground Check

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundMask; // The layer mask for ground objects
    [SerializeField] private Transform _groundCheck; // The transform representing the position to check for ground
    [SerializeField] private float _groundCheckRadius = 0.4f; // The radius for ground check
     private bool isGrounded; // Flag indicating if the player is grounded

    // Check if the player is grounded
    private void Grounded()
    {
        isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundMask);
        if (isGrounded)
        {
            isGrounded = true;
            jumpsRemaining = numberOfJumps;
        }
    }

    #endregion

    #region Walking

    [Header("Movement Speeds")]
    [SerializeField] private float _moveSpeed = 10; // Speed of regular walking
    [SerializeField] private float _sprintSpeed = 13; // Speed of sprinting
    [SerializeField] AudioClip[] stepSounds;
    private float horizontal; // Horizontal input from player

    private bool facingRight = true; // Flag indicating if the player is facing right
    // Handle player walking
    private void Walking()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Adjust velocity based on sprint input
        if (Input.GetButton("Sprint"))
        {
            rb.velocity = new Vector2(horizontal * _sprintSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(horizontal * _moveSpeed, rb.velocity.y);
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

    private void PlayFootstepSound()
    {
        AudioManager.Instance.PlayRandomSoundFXClip(stepSounds, transform, 0.5f);
    }
    private void FlipSprite()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    #endregion

    #region Jumping


    [Header("Jump Settings")]
    [SerializeField] private bool _playerCanDoubleJump = true; // Flag indicating if the player can jump
    [SerializeField] private bool _fixedJumpHeight = false; // Flag indicating if the player can jump
    [SerializeField] private float _coyoteTime = 0.2f; // Duration of grace period for jumping after leaving ground
    [SerializeField] private float _jumpBufferTime = 0.1f; // Duration of buffer time for jumping
    [SerializeField] private float _jumpSpeed = 12; // Height of the jump
    [SerializeField] private float _fallSpeed = 7; // Speed of falling
    [SerializeField] private float _jumpVelocityFalloff = 8; // Rate of decrease in jump velocity
    [SerializeField] AudioClip [] _jumpSounds;

    private int numberOfJumps = 1; // Number of jumps the player can perform
    private int jumpsRemaining; // Number of jumps remaining
    private float jumpBufferCounter; // Counter for jump buffer time
    private float coyoteTimeCounter; // Counter for coyote time

    private bool hasJumped; // Flag indicating if the player has initiated a jump for animations
    private bool hasLanded; // Flag indicating if the player has initiated a jump for animations
    
    // Handle player jumping
    private void Jumping()
    {
        // Reset coyote time counter if the player is grounded
        if (isGrounded)
        {
            coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Coyote time jump (first jump off the ground)
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && hasJumped == false)
        {
            rb.velocity = new Vector2(rb.velocity.x, _jumpSpeed);
            hasJumped = true;
            hasLanded = false;
            PlayJumpSound();
        }

        if(isGrounded && rb.velocity.y < 0)
        {
            hasJumped = false;
            if(!hasLanded)
            {
                hasLanded = true;
                PlayFootstepSound();
            }
        }


        if (Input.GetButtonUp("Jump"))
        {
            coyoteTimeCounter = 0; // Reset coyote time counter if the player releases the jump button to stop accidental double jumps
        }

        if (_fixedJumpHeight)
        {
            // Apply gravity and falloff to jump velocity
            if (rb.velocity.y < _jumpVelocityFalloff || rb.velocity.y > 0)
            {
                rb.velocity += Vector2.up * (_fallSpeed * Physics.gravity.y * Time.deltaTime); //FIXED JUMP HEIGHT - gravity is applied to the jump velocity

            }
        }
        else
        {
            // Apply gravity and falloff to jump velocity
            if (rb.velocity.y < _jumpVelocityFalloff || rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * (_fallSpeed * Physics.gravity.y * Time.deltaTime); //VARIABLE JUMP HEIGHT - gravity is applied to the jump velocity

            }
        }
    


        if (!_playerCanDoubleJump) return;

        // Mid air jumps - if the player is not grounded and has jumps remaining they can jump again
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter < 0 && jumpsRemaining > 0 && !isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, _jumpSpeed);
            PlayJumpSound();
            jumpsRemaining--;
        }

    }

    private void PlayJumpSound()
    {
        AudioManager.Instance.PlayRandomSoundFXClip(_jumpSounds, transform, 0.5f);
    }
    #endregion

    #region Wall Jumping
    [Header("General Wall Settings")]
    [SerializeField] private bool _playerCanWallJump = true; // Flag indicating if the player can wall jump
    [SerializeField] private LayerMask _wallMask; // The layer mask for wall objects
    [SerializeField] private Transform _wallCheck; // The transform representing the position to check for walls
    private bool isTouchingWall; // Flag indicating if the player is touching a wall

    [Header("Wall Slide Settings")]
    [SerializeField] private float _wallSlideSpeed = 2f; // Speed of sliding down a wall
    private bool isWallSliding; // Flag indicating if the player is sliding down a wall

    [Header("Wall Jump Settings")]
    [SerializeField] private float _wallJumpingTime = 0.2f;
    [SerializeField] private float _wallJumpingDuration = 0.2f;
    [SerializeField] private Vector2 _wallJumpingPower = new Vector2(8f, 16f);
    [SerializeField] private AudioClip[] _wallJumpSounds;
    private bool isWallJumping; // Flag indicating if the player is wall jumping
    private float wallJumpingDir;
    private float wallJumpingCounter;

    private void WallJumping()
    {
        if (!_playerCanWallJump) return;

        isTouchingWall = Physics2D.OverlapCircle(_wallCheck.position, _groundCheckRadius, _wallMask);


        WallSlide();

        WallJump();
    }

    private void WallSlide()
    {
        // If the player is touching a wall and not grounded and moving horizontally into the wall, slide down the wall
        if (isTouchingWall && !isGrounded && horizontal != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -_wallSlideSpeed, float.MaxValue));
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;

            wallJumpingDir = -transform.localScale.x;
            wallJumpingCounter = _wallJumpingTime; // gives a small buffer time to jump off the wall

            CancelInvoke("StopWallJumping");
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDir * _wallJumpingPower.x, _wallJumpingPower.y);
            wallJumpingCounter = 0;
            if (transform.localScale.x != wallJumpingDir)
            {
                FlipSprite();
            }
            AudioManager.Instance.PlayRandomSoundFXClip(_wallJumpSounds, transform, 0.5f);
            Invoke("StopWallJumping", _wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    #endregion

    #region Dashing

    [Header("Dash Settings")]
    [SerializeField] private bool _playerCanDash = true; // Flag indicating if the player can dash
    [SerializeField] private float _dashPower = 24f; // Power/speed of the dash
    [SerializeField] private float _dashDuration = 0.2f; // How long the dash lasts
    [SerializeField] private float _dashCooldown = 1f; // Cooldown time between dashes
    [SerializeField] private AudioClip[] _dashSounds;

    private bool canDash = true; // Flag indicating if the player can dash
    private bool isDashing = false; // Flag indicating if the player is in the middle of dashing

    private void Dashing()
    {
        if (!_playerCanDash) return;
        // If the player presses the dash button and can dash, start the dash coroutine
        if (Input.GetButtonDown("Dash") && canDash && !uninterruptibleAnim)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        AudioManager.Instance.PlayRandomSoundFXClip(_dashSounds, transform, 0.5f);
        canDash = false; // Prevent the player from dashing again until the cooldown is over - is set to true in the grounded function
        isDashing = true; // Sets flag that the player is currently dashing
        float originalGravity = rb.gravityScale; // Store the original gravity scale of the player
        rb.gravityScale = 0; // Set gravity to 0 so the player doesn't fall during the dash
        rb.velocity = new Vector2(transform.localScale.x * _dashPower, 0f); // Apply the dash power to the player
        ChangeAnimationState(PLAYER_DASH);
        trailRenderer.emitting = true; // Enable the trail renderer for the dash

        yield return new WaitForSeconds(_dashDuration); // Wait for the dash duration to end

        rb.gravityScale = originalGravity; // Reset the gravity scale
        isDashing = false; // Reset the dashing flag
        trailRenderer.emitting = false; // Disable the trail renderer

        yield return new WaitForSeconds(_dashCooldown); // Wait for the dash cooldown to end
        canDash = true; // Allow the player to dash again
    }
    #endregion

    #region Animation

    //animation states    
    const string PLAYER_WALK = "Player Walk";
    const string PLAYER_RUN = "Player Run";
    const string PLAYER_IDLE = "Player Idle";
    const string PLAYER_RISE = "Player Jump Rising";
    const string PLAYER_FALL = "Player Jump Falling";
    const string PLAYER_DASH = "Player Dash";
    const string PLAYER_ATTACK = "Player Attack";
    const string PLAYER_WALLSLIDE = "Player WallSlide";
    const string PLAYER_HURT = "Player Hurt";
    const string PLAYER_DEATH = "Player Death";


    private bool isDead = false;
    public bool isHurt = false;
    private Animator animator;
    private string currentState;
    bool uninterruptibleAnim = false;

    private void Animations()
    {
        if (playerCombat.isAttacking)
        {
            uninterruptibleAnim = true;
            return;
        }
        else
        {
            uninterruptibleAnim = false;
        }
        if (isWallSliding)
        {
            ChangeAnimationState(PLAYER_WALLSLIDE);
            return;
        }
        if(isHurt || isDead)
        {
            return;
        }
        GroundAnims();
        AirAnims();

    }

    private void GroundAnims()
    {
        if (!isGrounded) return;
        if (isDashing) return;

        if (horizontal != 0)
        {
            if (Input.GetButton("Sprint"))
            {
                ChangeAnimationState(PLAYER_RUN);
            }
            else
            {
                ChangeAnimationState(PLAYER_WALK);
            }
        }
        else
        {
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    private void AirAnims()
    {
        if (isGrounded) return;
        if (isDashing) return;

        if (rb.velocity.y > 0)
        {
            ChangeAnimationState(PLAYER_RISE);
        }
        else
        {
            ChangeAnimationState(PLAYER_FALL);
        }
    }

    public void PlayAttackAnim()
    {
        ChangeAnimationState(PLAYER_ATTACK);
    }
    public void PlayHurtAnim()
    {
        ChangeAnimationState(PLAYER_HURT);
        isHurt = true;
    }
    public void PlayDeathAnim()
    {
        ChangeAnimationState(PLAYER_DEATH);
        isDead = true;
    }
    private void HurtAnimFinished()
    {
        isHurt = false;
    }
    private void DeathAnimFinished()
    {
        gameObject.SetActive(false);
    }



    void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        animator.Play(newState);

        //reassign the current state
        currentState = newState;
    }

    #endregion
}
