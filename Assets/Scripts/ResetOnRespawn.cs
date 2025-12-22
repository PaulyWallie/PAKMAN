using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnRespawn : MonoBehaviour
{

     Vector3 startPosition;
     Quaternion startRotation;
     Vector3 startLocalScale;

     Rigidbody2D myRigidBody;
    
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startLocalScale = transform.localScale;

        if (GetComponent<Rigidbody2D>() != null)
        {
            myRigidBody = GetComponent<Rigidbody2D>();
        }
    }
    

    public void ResetObect()
    {
       // FindObjectOfType<EnemyHealth>().ResetHealth();
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startLocalScale;

        if (myRigidBody != null)
        {
            myRigidBody.linearVelocity = Vector3.zero;
        }
    }
}
