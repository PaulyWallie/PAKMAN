using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;

    public float moveSpeed;

    private Rigidbody2D myRigidBody;

    public bool movingRight;

    public GameObject slimes;
    private EnemyHealth enemyHealth;

    public GameObject bridge;
    public GameObject victory;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
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
            myRigidBody.velocity = new Vector3(moveSpeed, myRigidBody.velocity.y, 0);
        }
        else
        {
            myRigidBody.velocity = new Vector3(-moveSpeed, myRigidBody.velocity.y, 0);
        }
      

    }
    public void BossDamage()
    {
        enemyHealth.TakeDamage();
        Instantiate(slimes, gameObject.transform.position, Quaternion.identity);
        moveSpeed += 2;
       
       
        Debug.Log("Hurt");
        if (enemyHealth.currentHealth <= 0)
        {
            bridge.SetActive(true);
            victory.SetActive(true);
        }
    }
  
}
