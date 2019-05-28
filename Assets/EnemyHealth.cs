using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int MaxHealth = 1;
    public int currentHealth; 

    public GameObject deathSplosion;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = MaxHealth;
    }
    public void TakeDamage()
    {
        currentHealth--;
        FindObjectOfType<PlayerController>().Bounce();

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);

            Instantiate(deathSplosion, transform.position, transform.rotation);
        }
    }
    public void ResetHealth()
    {
        currentHealth = MaxHealth;
    }
}
