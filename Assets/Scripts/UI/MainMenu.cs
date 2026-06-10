using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public string levelSelect;
    public string[] levelNames;
    public int startingLives;

    private UIDocument uiDocument;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (uiDocument == null) return;
        var root = uiDocument.rootVisualElement;

        root.Q<Button>("newGameButton")?.RegisterCallback<ClickEvent>(ev => NewGame());
        root.Q<Button>("continueButton")?.RegisterCallback<ClickEvent>(ev => Continue());
        root.Q<Button>("quitButton")?.RegisterCallback<ClickEvent>(ev => QuitGame());
    }

    private void Start()
    {
        // AudioManager.current.PlayMenuMusic();
    }

    public void NewGame()
    {
        SceneManager.LoadScene(firstLevel);

        for (int i = 0; i < levelNames.Length; i++)
        {
            PlayerPrefs.SetInt(levelNames[i], 0);
        }

        PlayerPrefs.SetInt("CoinCount", 0);
        PlayerPrefs.SetInt("PlayerLives", startingLives);
        //AudioManager.current.PlayLevelMusic();
    }

    public void Continue()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

