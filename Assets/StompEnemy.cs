using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour
{
    public Rigidbody2D myRigidBody;

    public GameObject deathSplosion;
    public float bounceForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.SetActive(false);
            Instantiate(deathSplosion, other.transform.position, other.transform.rotation);
            myRigidBody.velocity = new Vector3(myRigidBody.transform.position.x, bounceForce, 0f);
        }
    }
}
