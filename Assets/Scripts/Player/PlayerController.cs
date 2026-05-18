using UnityEngine;

/// <summary>
/// Core controller for the Player. 
/// Handles physics and high-level logic, delegating specialized tasks to other components.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimationHandler), typeof(PlayerAudioHandler))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float bounceForce = 10f;
    
    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public float jumpForceMultiplier = 0.5f;
    
    [Header("Ground Check Settings")]
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    [Header("Knockback Settings")]
    public float knockBackLength = 0.2f;
    public float knockBackForce = 8f;
    public bool stopInput;

    private Rigidbody2D rb;
    private PlayerInputHandler input;
    private PlayerAnimationHandler animHandler;
    private PlayerAudioHandler audioHandler;
    private SpriteRenderer sr;

    private bool isGrounded;
    private bool isJumping;
    private bool canDoubleJump;
    private float knockBackCounter;
    private float knockBackDirection;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputHandler>();
        animHandler = GetComponent<PlayerAnimationHandler>();
        audioHandler = GetComponent<PlayerAudioHandler>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        CheckGroundStatus();
        CheckStomp();

        if (knockBackCounter <= 0)
        {
            HandleMovement();
            HandleJump();
        }
        else
        {
            HandleKnockBack();
        }

        animHandler.UpdateAnimation(isGrounded);
    }

    private void HandleMovement()
    {
        if (stopInput)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        Vector2 moveInput = input.MoveInput;
        rb.linearVelocity = new Vector2(moveSpeed * moveInput.x, rb.linearVelocity.y);

        if (moveInput.x < 0) sr.flipX = true;
        else if (moveInput.x > 0) sr.flipX = false;
    }

    private void HandleJump()
    {
        if (stopInput) return;

        if (input.JumpStarted)
        {
            if (isGrounded)
            {
                PerformJump();
            }
            else if (canDoubleJump)
            {
                PerformJump();
                canDoubleJump = false;
            }
        }

        if (input.JumpCanceled && isJumping && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpForceMultiplier);
            isJumping = false;
        }
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isJumping = true;
        audioHandler.PlayJump();
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);
        if (isGrounded)
        {
            canDoubleJump = true;
            if (rb.linearVelocity.y <= 0) isJumping = false;
        }
    }

    private void CheckStomp()
    {
        // Only stomp if falling
        if (rb.linearVelocity.y < -1f)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(groundCheckPoint.position, new Vector2(0.3f, 0.2f), 0f);
            foreach (var hit in hitEnemies)
            {
                if (hit.CompareTag("Enemy"))
                {
                    EnemyController enemy = hit.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(1);
                        Bounce();
                        break;
                    }
                }
            }
        }
    }

    private void HandleKnockBack()
    {
        knockBackCounter -= Time.deltaTime;
        rb.linearVelocity = new Vector2(knockBackDirection * knockBackForce, rb.linearVelocity.y);
    }

    public void KnockBack(Vector3 sourcePosition)
    {
        knockBackCounter = knockBackLength;
        knockBackDirection = transform.position.x < sourcePosition.x ? -1f : 1f;
        rb.linearVelocity = new Vector2(knockBackDirection * knockBackForce, knockBackForce);
        
        animHandler.TriggerHurt();
        audioHandler.PlayHurt();
    }

    public void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
        audioHandler.PlayJump(); 
    }
}

