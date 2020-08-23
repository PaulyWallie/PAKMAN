using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour
{
    public GameObject deathEffect;

    public GameObject Collectable;

    [Range(0, 100)]
    public float chanceToDrop;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.transform.parent.gameObject.SetActive(false);
           /// Instantiate(deathEffect, other.transform.position, other.transform.rotation);

            PlayerController.instance.Bounce();

            float dropSelect = Random.Range(0, 100);

            if (dropSelect <= chanceToDrop)
            {
                Instantiate(Collectable, other.transform.position, other.transform.rotation);
            }
            //AudioManager.instance.PlaySFX();
        }
    }
}

