using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public GameObject ObjectToMove;

    public Transform startPoint;
    public Transform endPoint;

    public float moveSpeed;

    private Vector3 currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        currentTarget = endPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        ObjectToMove.transform.position = Vector3.MoveTowards(ObjectToMove.transform.position, currentTarget, moveSpeed * Time.deltaTime);

        if (ObjectToMove.transform.position == endPoint.position)
        {
            currentTarget = startPoint.position;
        }

        if (ObjectToMove.transform.position == startPoint.position)
        {
            currentTarget = endPoint.position;
        }
    }
}
