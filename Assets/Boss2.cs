using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public GameObject victory;
    public float moveSpeed;

    public bool movingRight;
  
    enum bossState {BossDamageOne, BossDamageTwo, BossDamageThree, BossDied};
    bossState currentState = bossState.BossDamageOne;
    Rigidbody2D rigidBody; 
    Animator anim;
    SpriteRenderer spriteRenderer;
    EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingRight && transform.position.x > rightPoint.position.x)
        {
            movingRight = false;
        }
        if (!movingRight && transform.position.x < leftPoint.position.x)
        {
            movingRight = true;
        }
        if (movingRight)
        {
            rigidBody.velocity = new Vector3(moveSpeed, rigidBody.velocity.y, 0);
            spriteRenderer.flipX = false;
        }
        else
        {
            rigidBody.velocity = new Vector3(-moveSpeed, rigidBody.velocity.y, 0);
            spriteRenderer.flipX = true;
        }
        anim.SetBool("Walk", true);
    }
   
    public void BossDamage()
    {
        GetComponent<EnemyHealth>().TakeDamage();
        currentState++;
        switch (currentState)
        {
            case bossState.BossDamageTwo:
                anim.SetBool("Phase2", true);
                moveSpeed++;
                break;

            case bossState.BossDamageThree:
                anim.SetBool("Phase3", true);
                anim.SetBool("Phase2", false);
                moveSpeed++;
                break;
            case bossState.BossDied:
                anim.SetBool("Phase3",false);
                rigidBody.velocity = Vector2.zero;
                anim.SetTrigger("BossEnd");
                break;

        }
                if (enemyHealth.currentHealth <= 0)
                {
                    victory.SetActive(true);
                }
    }
}