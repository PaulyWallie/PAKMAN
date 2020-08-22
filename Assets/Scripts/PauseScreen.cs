using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public string levelSelect;
    public string mainMenu;

    private LevelManager theLevelManager;

    public GameObject thePauseScreen;

    private PlayerController thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindObjectOfType<LevelManager>();
        thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale == 0f)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        thePauseScreen.SetActive(true);
       // thePlayer.canMove = false;
        //AudioManager.current.PlayMenuMusic();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        thePauseScreen.SetActive(false);
        //thePlayer.canMove = true;
        //AudioManager.current.PlayLevelMusic();
    }

    public void LevelSelect()
    {
       // PlayerPrefs.SetInt("CoinCount", theLevelManager.coinCount);
        //PlayerPrefs.SetInt("PlayerLives", theLevelManager.currentLives);

        Time.timeScale = 1f;
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
      
    }
}
