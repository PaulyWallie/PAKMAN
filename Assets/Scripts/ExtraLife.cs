using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    public int livesToGive;
    private LevelManager theLevelManager;
    private AudioManager AudioManager;
    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        AudioManager = FindObjectOfType<AudioManager>();
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag =="Player")
        {
            theLevelManager.AddLives(livesToGive);
            AudioManager.PlayextaLifeAudio();
            gameObject.SetActive(false);

        }
    }
}
