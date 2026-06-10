using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseScreen : MonoBehaviour
{
    public string levelSelect;
    public string mainMenu;

    private LevelManager theLevelManager;
    public GameObject thePauseScreen;
    private PlayerController thePlayer;

    private InputAction pauseAction;

    // Start is called before the first frame update
    void Start()
    {
        theLevelManager = FindAnyObjectByType<LevelManager>();
        thePlayer = FindAnyObjectByType<PlayerController>();

        if (InputSystem.actions != null)
        {
            pauseAction = InputSystem.actions.FindAction("Pause");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction != null && pauseAction.WasPressedThisFrame())
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

        if (UITKController.instance != null)
            UITKController.instance.ShowPauseMenu(true);
        else
            thePauseScreen.SetActive(true);
       // thePlayer.canMove = false;
        //AudioManager.current.PlayMenuMusic();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;

        if (UITKController.instance != null)
            UITKController.instance.ShowPauseMenu(false);
        else
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
