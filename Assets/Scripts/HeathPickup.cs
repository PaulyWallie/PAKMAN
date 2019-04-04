using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathPickup : MonoBehaviour
{
    public int healthToGive;

    private LevelManager thelevelManager;

    // Start is called before the first frame update
    void Start()
    {
        thelevelManager = FindObjectOfType<LevelManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            thelevelManager.GiveHeath(healthToGive);
            gameObject.SetActive(false); 
        }
    }
}
