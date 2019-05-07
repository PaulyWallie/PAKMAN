using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlime : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;

    public int maxHealth;
    private int currentHealth;

    public float moveSpeed;
    public float flash;

    private Rigidbody2D myRigidBody;

    public bool movingRight;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }


    // Update is called once per frame
    void Update()
    {

        if (currentHealth <= 0)
            gameObject.SetActive(false);
        
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
     public void TakeDamage()
    {
        currentHealth--;
        StartCoroutine(HurtCO());
    }
    IEnumerator HurtCO()
    {
        moveSpeed++;

        yield return new  WaitForSeconds(flash);

    }
}
