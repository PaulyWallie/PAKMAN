using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public Transform leftPoint;
    public Transform rightPoint;

    public float moveSpeed;

    private Rigidbody2D myRigidBody;

    public bool movingRight;
 
    // Start is called before the first frame update
    void Start() 
    {
        myRigidBody = GetComponent<Rigidbody2D>();
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
            myRigidBody.linearVelocity = new Vector3(moveSpeed, myRigidBody.linearVelocity.y, 0);
        }
        else
        {
            myRigidBody.linearVelocity = new Vector3(-moveSpeed, myRigidBody.linearVelocity.y, 0);
        }
    }
}