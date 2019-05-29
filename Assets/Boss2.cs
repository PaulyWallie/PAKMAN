using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;
    public float moveSpeed;

    public bool movingRight;

    enum bossState {BossDamageOne, BossDamageTwo, BossDamageThree};
    Rigidbody2D rigidbody; 
    Animator anim;
    SpriteRenderer spriteRenderer;
    EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        //bossState.BossDamageOne;
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            rigidbody.velocity = new Vector3(moveSpeed, rigidbody.velocity.y, 0);
            spriteRenderer.flipX = false;
        }
        else
        {
            rigidbody.velocity = new Vector3(-moveSpeed, rigidbody.velocity.y, 0);
            spriteRenderer.flipX = true;

        }
    }

    public void BossDamage()
    {
        enemyHealth.TakeDamage();
        //bossState++;
    }
}