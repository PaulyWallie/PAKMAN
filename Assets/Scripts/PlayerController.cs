using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("Movement Settings")]
    public float moveSpeed;
    public float bounceForce;
    public float jumpForce;

    [Header("Ground Check Settings")]
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    [Header("Knockback Settings")]
    public float knockBackLength, knockBackForce;
    public bool stopInput;


    public Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    Vector2 moveInput;
    bool isGrounded;
    bool canDoubleJump;
    float knockBackCounter;
    void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        var playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;
    }

    void OnDisable()
    {
        var playerInput = GetComponent<PlayerInput>();
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    void Update()
    {
        if (knockBackCounter <= 0)
        {
            HandleMovement();
            CheckGroundStatus();
        }
        else
        {
            HandleKnockBack();
        }
        UpdateAnimation();
    }
    void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveSpeed * moveInput.x, rb.linearVelocity.y);

        if (moveInput.x < 0)
        {
            sr.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            sr.flipX = false;
        }
    }
    void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);

        if (isGrounded)
        {
            canDoubleJump = true;
        }
    }
    void HandleKnockBack()
    {
        knockBackCounter -= Time.deltaTime;
        float knockBackDirection = Mathf.Sign(moveInput.x);
        rb.linearVelocity = new Vector2(knockBackDirection * knockBackForce, rb.linearVelocity.y);
    }


    void UpdateAnimation()
    {
        anim.SetFloat("moveSpeed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }
    void OnActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move":
                moveInput = context.ReadValue<Vector2>();
                print("isMoving");
                break;
            case "Jump":
                if (context.performed)
                { 
                    HandleJump();
                }
                break;
            default:
                print($"Unhandled action: {context.action.name}");
                break;
        }
    }
    void HandleJump()
    {
        if (isGrounded)
            Jump();
            //AudioManager.instance()
        
        else if (canDoubleJump)
        {
            Jump();
            canDoubleJump = false;
            //AudioManager.instance()
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        rb.linearVelocity = new Vector2(0f, knockBackForce);

        anim.SetTrigger("Hurt");
    }

    public void Bounce()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
        //AudioManager.intance.PlaySFX();
    }
}
