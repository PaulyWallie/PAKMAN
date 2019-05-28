using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BossTrigger : MonoBehaviour
{
 
    public GameObject Boss;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Boss.SetActive(true);
            Destroy(this);
        }
    }
}
