using UnityEngine;

// < summary >
// Core controller for the Player.
// Handles movement, jumping, knockback, and stomp mechanics.
// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAnimationHandler), typeof(PlayerAudioHandler))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float bounceForce = 10f;

    [Tooltip("Allows jumping shortly after leaving the ground.")]
    [SerializeField] private float coyoteTime = 0.15f;

    [Tooltip("Allows jump input shortly before landing.")]
    [SerializeField] private float jumpBufferTime = 0.15f;

    [Tooltip("Higher = faster falling.")]
    [SerializeField] private float fallMultiplier = 2.5f;

    [Tooltip("Higher = shorter jump when releasing early.")]
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Stomp")]
    [SerializeField] private Vector2 stompBoxSize = new Vector2(.3f, .2f);
    [SerializeField] private LayerMask enemyLayer;

    [Header("Knockback")]
    [SerializeField] private float knockBackLength = .2f;
    [SerializeField] private float knockBackForce = 8f;

    public bool stopInput;

    private Rigidbody2D rb;
    private PlayerInputHandler input;
    private PlayerAnimationHandler animHandler;
    private PlayerAudioHandler audioHandler;
    private SpriteRenderer sr;

    private bool isGrounded;
    private bool canDoubleJump;

    private float coyoteCounter;
    private float jumpBufferCounter;

    private float knockBackCounter;
    private float knockBackDirection;

    private void Awake()
    {
        Instance = this;

        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputHandler>();
        animHandler = GetComponent<PlayerAnimationHandler>();
        audioHandler = GetComponent<PlayerAudioHandler>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        CheckGround();

        HandleJumpInput();

        animHandler.UpdateAnimation(isGrounded);
    }

    private void FixedUpdate()
    {
        ApplyBetterGravity();

        CheckStomp();

        if (knockBackCounter > 0)
        {
            HandleKnockback();
        }
        else
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if (stopInput)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        Vector2 velocity = rb.linearVelocity;
        velocity.x = input.MoveInput.x * moveSpeed;
        rb.linearVelocity = velocity;

        if (input.MoveInput.x != 0)
            sr.flipX = input.MoveInput.x < 0;
    }

    private void HandleJumpInput()
    {
        if (stopInput)
            return;

        if (input.JumpStarted)
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0)
        {
            if (coyoteCounter > 0)
            {
                Jump();

                jumpBufferCounter = 0;
                coyoteCounter = 0;
            }
            else if (canDoubleJump)
            {
                Jump();

                canDoubleJump = false;
                jumpBufferCounter = 0;
            }
        }
    }

    private void Jump()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;

        audioHandler.PlayJump();
    }

    private void ApplyBetterGravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (fallMultiplier - 1) *
                Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && input.JumpCanceled)
        {
            rb.linearVelocity += Vector2.up *
                Physics2D.gravity.y *
                (lowJumpMultiplier - 1) *
                Time.fixedDeltaTime;
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(
            groundCheckPoint.position,
            groundRadius,
            whatIsGround);

        if (isGrounded)
            canDoubleJump = true;
    }

    private void CheckStomp()
    {
        if (rb.linearVelocity.y >= -1f)
            return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            groundCheckPoint.position,
            stompBoxSize,
            0f,
            enemyLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out EnemyController enemy))
            {
                enemy.TakeDamage(1);
                Bounce();
                break;
            }
        }
    }

    private void HandleKnockback()
    {
        knockBackCounter -= Time.fixedDeltaTime;

        rb.linearVelocity = new Vector2(
            knockBackDirection * knockBackForce,
            rb.linearVelocity.y);

        animHandler.TriggerHurt();
    }

    public void KnockBack(Vector3 sourcePosition)
    {
        knockBackCounter = knockBackLength;

        knockBackDirection =
            transform.position.x < sourcePosition.x ? -1 : 1;

        rb.linearVelocity = new Vector2(
            knockBackDirection * knockBackForce,
            knockBackForce);

        audioHandler.PlayHurt();
    }

    public void Bounce()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            bounceForce);

        audioHandler.PlayJump();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPoint.position, stompBoxSize);
    }
}