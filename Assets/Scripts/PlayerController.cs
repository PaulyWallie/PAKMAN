using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    private float activeMoveSpeed;

    public bool canMove;


    public float jumpSpeed;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public bool isGrounded;

    public Rigidbody2D myRigidBody;
    private Animator myAnim;
    private LevelManager theLevelManager;
    private SlimeController slimeController;
    private AudioManager audioManager;

    public Vector2 respawnPosition;

   
    public float knockbackForce;
    public float knockbackLeagth;
    private float knockbackCounter;

    public float inviaibiltyLength;
    private float invicibilityCounter;

    private bool onPlatform;
    public float onPlatformSpeedModifier;

    public int damageToGive;

    public float bounceForce;

    public GameObject deathSplosion;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        theLevelManager = FindObjectOfType <LevelManager>();
        slimeController = FindObjectOfType<SlimeController>();
        audioManager = FindObjectOfType<AudioManager>();
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
                myRigidBody.velocity = new Vector2(activeMoveSpeed, myRigidBody.velocity.y);
                transform.localScale = new Vector2(1f, 1f);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0f)
            {
                myRigidBody.velocity = new Vector2(-activeMoveSpeed, myRigidBody.velocity.y);
                transform.localScale = new Vector2(-1f, 1f);
            }
            else
            {
                myRigidBody.velocity = new Vector2(0f, myRigidBody.velocity.y);
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpSpeed);
                audioManager.PlayJumpAudio();
            }
        }

        if (knockbackCounter > 0)
        {
            knockbackCounter -= Time.deltaTime;
            if (transform.localScale.x > 0)
            {
                myRigidBody.velocity = new Vector2(-knockbackForce, knockbackForce);
            }
            else
            {
                myRigidBody.velocity = new Vector2(knockbackForce, knockbackForce);
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
        if (transform.position.y < -9)
        {
            theLevelManager.Respawn();
        }


        myAnim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));
        myAnim.SetBool("Grounded", isGrounded);
    }

    public void Knockback()
    {
        knockbackCounter = knockbackLeagth;
        invicibilityCounter = inviaibiltyLength;
        theLevelManager.invinciable = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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

        if (other.gameObject.tag == "Enemy")
        {
            foreach (ContactPoint2D point in other.contacts)
            {
                Debug.DrawLine(point.point, point.point + point.normal, Color.red, 10);
                if (point.normal.y > 0.9)
                {
                    other.gameObject.SetActive(false);
                    Instantiate(deathSplosion, other.transform.position, other.transform.rotation);
                    myRigidBody.velocity = new Vector3(myRigidBody.transform.position.x, bounceForce, 0f);
                }
            }
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
