using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public Rigidbody2D theRB;
    public float jumpForce;

    bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

     bool canDoupleJump;

     SpriteRenderer theSR;
     Animator anim;

    public float knockBackLength, knockBackForce;
     float knockBackCounter; 

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
        if (knockBackCounter <= 0)
        {
            theRB.linearVelocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.linearVelocity.y);

            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);

            if (isGrounded)
            {
                canDoupleJump = true;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
                    //AudioManager.instance()
                }
                else
                {
                    if (canDoupleJump)
                    {
                        theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
                        canDoupleJump = false;
                        //AudioManager.instance()
                    }
                }
            }

            if (theRB.linearVelocity.x < 0)
            {
                theSR.flipX = true;
            }
            else if (theRB.linearVelocity.x > 0)
            {
                theSR.flipX = false;
            }
            else
            {
                knockBackCounter -= Time.deltaTime;
                if (!theSR.flipX)
                {
                    theRB.linearVelocity = new Vector2(-knockBackForce, theRB.linearVelocity.y);
                }
                else
                {
                    theRB.linearVelocity = new Vector2(knockBackForce, theRB.linearVelocity.y);
                }
            }
        }

        anim.SetFloat("moveSpeed", Mathf.Abs(theRB.linearVelocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }
    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        theRB.linearVelocity = new Vector2(0f, knockBackForce);


        anim.SetTrigger("Hurt");
    }

    public void Bounce()
    {
        theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, bounceForce);
        //AudioManager.intance.PlaySFX();
    } 
}
   