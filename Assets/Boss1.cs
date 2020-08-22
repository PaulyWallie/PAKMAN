using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public GameObject entrance;
    public Transform leftPoint;
    public Transform rightPoint;

    public float moveSpeed;

    Rigidbody2D myRigidBody;
    Animator anim;
    SpriteRenderer spriteRenderer;

    public bool movingRight;

    public GameObject slimes;
    private EnemyHealth enemyHealth;

    public GameObject bridge;
    public GameObject victory;
    public GameObject flyingDirt;
   


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = 0;
        movingRight = false;
        //  Instantiate(entrance, gameObject.transform.parent);
        // StartCoroutine(Entrance());
    }

    IEnumerator Entrance()
    {
        moveSpeed = 0;
        yield return new WaitForSeconds( 3f);
        moveSpeed = 3;


    }
    // Update is called once per frame
    void Update()
    {
    
        Moving();
    }

    private void Moving()
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
            myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, myRigidBody.velocity.y);
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void BossDamage()
    {
        enemyHealth.TakeDamage();
        Instantiate(slimes, gameObject.transform.position, Quaternion.identity);
        moveSpeed += 2;
        transform.localScale = new Vector2(1.5f, 1.5f);

        if (enemyHealth.currentHealth <= 0)
        {
            bridge.SetActive(true);
            victory.SetActive(true);
        }
    }
}
