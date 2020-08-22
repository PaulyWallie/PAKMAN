using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeathPickup : MonoBehaviour
{
    public int healthToGive;
    private AudioManager audioManager;
    private LevelManager thelevelManager;
    // Start is called before the first frame update
    void Start()
    {
        thelevelManager = FindObjectOfType<LevelManager>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //thelevelManager.GiveHeath(healthToGive);
           // audioManager.PlayHeartPickupAudio();
            gameObject.SetActive(false); 
        }
    }
}
