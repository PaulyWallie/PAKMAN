using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour
{
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            FindObjectOfType<EnemyHealth>().TakeDamage();
        }
        if (other.tag =="Boss1")
        {
            FindObjectOfType<Boss1>().BossDamage();
        }
       
    }
}
