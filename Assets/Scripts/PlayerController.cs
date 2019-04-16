using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    private float activeMoveSpeed;

    public bool canMove;

    public Rigidbody2D myRigidBody;

    public float jumpSpeed;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public bool isGrounded;

    private Animator myAnim;

    public Vector3 respawnPosition;

    public LevelManager theLevelManager;

    public GameObject stompBox;
    public float knockbackForce;
    public float knockbackLeagth;
    private float knockbackCounter;

    public float inviaibiltyLength;
    private float invicibilityCounter;

    private bool onPlatform;
    public float onPlatformSpeedModifier;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        theLevelManager = FindObjectOfType <LevelManager>();

        respawnPosition = transform.position;

        activeMoveSpeed = moveSpeed;

        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (knockbackCounter <= 0 && canMove)
        {

            if (onPlatform)
            {
                activeMoveSpeed = moveSpeed * onPlatformSpeedModifier;
            }
            else
            {
                activeMoveSpeed = moveSpeed;
            }


                if (Input.GetAxisRaw("Horizontal") > 0f)
                {
                    myRigidBody.velocity = new Vector3(activeMoveSpeed, myRigidBody.velocity.y, 0f);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (Input.GetAxisRaw("Horizontal") < 0f)
                {
                    myRigidBody.velocity = new Vector3(-activeMoveSpeed, myRigidBody.velocity.y, 0f);
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    myRigidBody.velocity = new Vector3(0f, myRigidBody.velocity.y, 0f);
                }

                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, jumpSpeed, 0);
                    
                }
            }

            if (knockbackCounter > 0)
            {
                knockbackCounter -= Time.deltaTime;
                if (transform.localScale.x > 0)
                {
                    myRigidBody.velocity = new Vector3(-knockbackForce, knockbackForce, 0f);
                }
                else
                {
                    myRigidBody.velocity = new Vector3(knockbackForce, knockbackForce, 0f);
                }
            }

            if (invicibilityCounter > 0)
            {
                invicibilityCounter -= Time.deltaTime;
            }
            if (invicibilityCounter <= 0)
            {
                theLevelManager.invinciable = false;
            }


            myAnim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));
            myAnim.SetBool("Grounded", isGrounded);

            if (myRigidBody.velocity.y < 0)
            {
                stompBox.SetActive(true);
            }
            else
            {
                stompBox.SetActive(false);

            }
        }

    public void Knockback()
    {
        knockbackCounter = knockbackLeagth;
        invicibilityCounter = inviaibiltyLength;
        theLevelManager.invinciable = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillPlane")
        { 
            theLevelManager.Respawn();
        }
        if (other.tag == "Checkpoint")
        {
            respawnPosition = other.transform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
            onPlatform = true;
        }    
    }

     void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
            onPlatform = false;
        }
    }
}
