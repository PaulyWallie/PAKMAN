using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public float moveSpeed;
    private bool canMove;
    private PlayerController thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
           transform.position = Vector3.MoveTowards(transform.position, thePlayer.transform.position, moveSpeed *Time.deltaTime);
        }
    }
    void OnBecameVisible()
    {
        canMove = true;
    }

     void OnEnable()
    {
        canMove = false;
    }
}
