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

    [SerializeField] private Sprite _playerIcon;
    [SerializeField] private Sprite _heartIcon;
    private Label coinLabel;
    private Label skullLabel;
    private Label livesLabel;
    private VisualElement[] hearts;
    private PauseScreen _pauseScreen;
    private GameOver _gameOver;
    private Button _resumeButton, _pauseLevelButton, _pauseQuitButton;
    private Button _restartButton, _gameLevelButton, _gameQuitButton;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        _pauseScreen = GetComponent<PauseScreen>();
        _gameOver = GetComponent<GameOver>();
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
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
        var playerImage = root.Q<Image>("playerLivesIcon");
        if (playerImage != null) playerImage.sprite = _playerIcon;
        foreach (var heart in hearts)
            if (heart != null) heart.Query<Image>().ForEach(image => image.sprite = _heartIcon);
    }

    private void InitializePauseMenu()
    {
        if (pauseMenuDocument == null) return;
        var root = pauseMenuDocument.rootVisualElement;
        root.style.display = DisplayStyle.None;
        _resumeButton = root.Q<Button>("resumeButton");
        _pauseLevelButton = root.Q<Button>("levelSelectButton");
        _pauseQuitButton = root.Q<Button>("quitButton");
        if (_resumeButton != null) _resumeButton.clicked += Resume;
        if (_pauseLevelButton != null) _pauseLevelButton.clicked += PauseLevelSelect;
        if (_pauseQuitButton != null) _pauseQuitButton.clicked += PauseQuit;
    }

    private void InitializeGameOver()
    {
        if (gameOverDocument == null) return;
        var root = gameOverDocument.rootVisualElement;
        root.style.display = DisplayStyle.None;
        _restartButton = root.Q<Button>("restartButton");
        _gameLevelButton = root.Q<Button>("levelSelectButton");
        _gameQuitButton = root.Q<Button>("quitButton");
        if (_restartButton != null) _restartButton.clicked += Restart;
        if (_gameLevelButton != null) _gameLevelButton.clicked += GameLevelSelect;
        if (_gameQuitButton != null) _gameQuitButton.clicked += GameQuit;
    }

    private void OnDisable()
    {
        if (_resumeButton != null) _resumeButton.clicked -= Resume;
        if (_pauseLevelButton != null) _pauseLevelButton.clicked -= PauseLevelSelect;
        if (_pauseQuitButton != null) _pauseQuitButton.clicked -= PauseQuit;
        if (_restartButton != null) _restartButton.clicked -= Restart;
        if (_gameLevelButton != null) _gameLevelButton.clicked -= GameLevelSelect;
        if (_gameQuitButton != null) _gameQuitButton.clicked -= GameQuit;
    }

    private void Resume() { if (_pauseScreen != null) _pauseScreen.ResumeGame(); }
    private void PauseLevelSelect() { if (_pauseScreen != null) _pauseScreen.LevelSelect(); }
    private void PauseQuit() { if (_pauseScreen != null) _pauseScreen.QuitToMainMenu(); }
    private void Restart() { if (_gameOver != null) _gameOver.Restart(); }
    private void GameLevelSelect() { if (_gameOver != null) _gameOver.LevelSelect(); }
    private void GameQuit() { if (_gameOver != null) _gameOver.QuitToMainMenu(); }

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

        if (livesLabel != null && LevelManager.Instance != null)
            livesLabel.text = $"× {LevelManager.Instance.CurrentLives}";

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
            if (hearts[i] == null) continue;
            int units = Mathf.Clamp(currentHealth - i * 2, 0, 2);
            hearts[i].EnableInClassList("heart-half", units == 1);
            hearts[i].EnableInClassList("heart-empty", units == 0);
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
