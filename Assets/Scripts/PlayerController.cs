using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public float jumpForce;
    public Rigidbody2D theRB;

    private bool isGrounded;
    public Transform checkGroundPoint;
    public LayerMask whatIsGround;

    private bool canDoupleJump;
    private SpriteRenderer theSR;

    private Animator anim;
    public float knockBackLength, knockBackForce;
    private float knockBackCounter;
    public float bounceForce;

    public bool stopInput;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (knockBackCounter<= 0)
        {
            theRB.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.velocity.y);

            isGrounded = Physics2D.OverlapCircle(checkGroundPoint.position, .2f, whatIsGround);

            if (isGrounded)
            {
                canDoupleJump = true;
            }

            if (Input.GetButton("Jump"))
            {
                if (isGrounded)
                {
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                    //AudioManager.instance()
                }
                else
                {
                    if (canDoupleJump)
                    {
                        theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                        canDoupleJump = false;
                        //AudioManager.instance()
                    }
                }
            }

            if (theRB .velocity.x < 0)
            {
                theSR.flipX = true;
            }
            else if (theRB.velocity.x > 0)
            {
                theSR.flipX = false;
            }
            else
            {
                knockBackForce -= Time.deltaTime;
                if (!theSR.flipX)
                {
                    theRB.velocity = new Vector2(-knockBackForce, theRB.velocity.y);
                }
                else
                {
                    theRB.velocity = new Vector2(knockBackForce, theRB.velocity.y);
                }
            }
        }

        anim.SetFloat("moveSpeed", Mathf.Abs(theRB.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }
    public void KnockBack()
    {
        knockBackLength = knockBackCounter;
        theRB.velocity = new Vector2(0f, knockBackForce);

        anim.SetTrigger("Hurt");
    }

    public void Bounce()
    {
        theRB.velocity = new Vector2(theRB.velocity.x, bounceForce);
        //AudioManager.intance.PlaySFX();
    } 
}
   