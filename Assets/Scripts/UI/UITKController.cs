using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class UITKController : MonoBehaviour
{
    public static UITKController instance;

    [Header("UI Documents")]
    public UIDocument hudDocument;
    public UIDocument pauseMenuDocument;
    public UIDocument gameOverDocument;

    private Label coinLabel;
    private Label skullLabel;
    private Label livesLabel;
    private VisualElement[] hearts;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        InitializeHUD();
        InitializePauseMenu();
        InitializeGameOver();
    }

    private void InitializeHUD()
    {
        if (hudDocument == null) return;
        var root = hudDocument.rootVisualElement;
        coinLabel = root.Q<Label>("coinLabel");
        skullLabel = root.Q<Label>("skullLabel");
        livesLabel = root.Q<Label>("livesLabel");
        
        hearts = new VisualElement[3];
        hearts[0] = root.Q<VisualElement>("heart1");
        hearts[1] = root.Q<VisualElement>("heart2");
        hearts[2] = root.Q<VisualElement>("heart3");
    }

    private void InitializePauseMenu()
    {
        if (pauseMenuDocument == null) return;
        var root = pauseMenuDocument.rootVisualElement;
        root.style.display = DisplayStyle.None;

        root.Q<Button>("resumeButton")?.RegisterCallback<ClickEvent>(ev => {
            var pauseScreen = FindAnyObjectByType<PauseScreen>();
            pauseScreen?.ResumeGame();
        });

        root.Q<Button>("levelSelectButton")?.RegisterCallback<ClickEvent>(ev => {
            var pauseScreen = FindAnyObjectByType<PauseScreen>();
            pauseScreen?.LevelSelect();
        });

        root.Q<Button>("quitButton")?.RegisterCallback<ClickEvent>(ev => {
            var pauseScreen = FindAnyObjectByType<PauseScreen>();
            pauseScreen?.QuitToMainMenu();
        });
    }

    private void InitializeGameOver()
    {
        if (gameOverDocument == null) return;
        var root = gameOverDocument.rootVisualElement;
        root.style.display = DisplayStyle.None;

        root.Q<Button>("restartButton")?.RegisterCallback<ClickEvent>(ev => {
            var gameOver = FindAnyObjectByType<GameOver>();
            gameOver?.Restart();
        });

        root.Q<Button>("levelSelectButton")?.RegisterCallback<ClickEvent>(ev => {
            var gameOver = FindAnyObjectByType<GameOver>();
            gameOver?.LevelSelect();
        });

        root.Q<Button>("quitButton")?.RegisterCallback<ClickEvent>(ev => {
            var gameOver = FindAnyObjectByType<GameOver>();
            gameOver?.QuitToMainMenu();
        });
    }

    private void Update()
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        if (LevelStats.instance != null)
        {
            if (coinLabel != null) coinLabel.text = "Coins: " + LevelStats.instance.coinsCollected;
            if (skullLabel != null) skullLabel.text = "Skulls: " + LevelStats.instance.skullsCollected;
        }

        // Lives and Hearts (assuming lives are in PlayerHealthController or LevelManager)
        var health = PlayerHealthController.instance;
        if (health != null)
        {
            UpdateHearts(health.currentHealth);
        }
    }

    public void UpdateHearts(int currentHealth)
    {
        if (hearts == null) return;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth) hearts[i].style.visibility = Visibility.Visible;
            else hearts[i].style.visibility = Visibility.Hidden;
        }
    }

    public void ShowPauseMenu(bool show)
    {
        if (pauseMenuDocument != null)
            pauseMenuDocument.rootVisualElement.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }

    public void ShowGameOver(bool show)
    {
        if (gameOverDocument != null)
            gameOverDocument.rootVisualElement.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
