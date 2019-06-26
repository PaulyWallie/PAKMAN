﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    private float activeMoveSpeed;

    public bool canMove;

    public float jumpSpeed;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;

    public float bounceForce;

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

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        theLevelManager = FindObjectOfType<LevelManager>();
        slimeController = FindObjectOfType<SlimeController>();
        audioManager = FindObjectOfType<AudioManager>();
        respawnPosition = transform.position;

        activeMoveSpeed = moveSpeed;
        jumpTimeCounter = jumpTime;

        canMove = true;
        isJumping = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (knockbackCounter <= 0 && canMove)
        {
            OnPlatform();
            Movement();
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

    private void Movement()
    {
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
            isJumping = true;
            jumpTimeCounter = jumpTime;
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpSpeed);
            audioManager.PlayJumpAudio();
        }

        if (Input.GetButton("Jump") && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, jumpSpeed);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }

    private void OnPlatform()
    {
        if (onPlatform)
        {
            activeMoveSpeed = moveSpeed * onPlatformSpeedModifier;
        }
        else
        {
            activeMoveSpeed = moveSpeed;
        }
    }

    public void Knockback()
    {
        knockbackCounter = knockbackLeagth;
        invicibilityCounter = inviaibiltyLength;
        theLevelManager.invinciable = true;
    }

    public void Bounce()
    {
        myRigidBody.velocity = new Vector3(myRigidBody.transform.position.x, bounceForce, 0f);
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
