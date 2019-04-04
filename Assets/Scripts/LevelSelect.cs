using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public string levelToLoad;

    public bool unlocked;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Level1", 1);

        if(PlayerPrefs.GetInt(levelToLoad) ==1 )
        {
            unlocked = true;
        }
        else
        {
            unlocked = false;
        }

        if (unlocked)
        {

        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if(Input.GetButtonDown("Jump") && unlocked)
            {
                SceneManager.LoadScene(levelToLoad);
            }
        } 
    }
}
