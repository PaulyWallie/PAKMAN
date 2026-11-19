using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Responsible ONLY for managing high-level level flow (Respawning, Ending).
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public float waitToRespawn = 2f;

    public const int StartingLives = 3;
    public int CurrentLives { get; private set; }
    private bool isRespawning;
    private bool isGameOver;

    public static void ResetLives()
    {
        PlayerPrefs.SetInt("PlayerLives", StartingLives);
    }

    public void AddLives(int amount = 1)
    {
        if (isGameOver || amount <= 0) return;
        CurrentLives += amount;
        PlayerPrefs.SetInt("PlayerLives", CurrentLives);
    }

    [SerializeField, Tooltip("Require Skull pickups in this scene before the level exit works. No skulls means no requirement.")]
    private bool requireSkulls;

    private readonly System.Collections.Generic.HashSet<Pickup> remainingSkulls = new System.Collections.Generic.HashSet<Pickup>();
    private bool isEnding;

    public bool CanEndLevel => !requireSkulls || remainingSkulls.Count == 0;

    private void Awake()
    {
        Instance = this;
        CurrentLives = PlayerPrefs.GetInt("PlayerLives", StartingLives);
        if (CurrentLives <= 0) CurrentLives = StartingLives;
        PlayerPrefs.SetInt("PlayerLives", CurrentLives);
        foreach (GameObject root in gameObject.scene.GetRootGameObjects())
        {
            foreach (Pickup pickup in root.GetComponentsInChildren<Pickup>(true))
            {
                if (pickup.pickupType == PickupType.LevelSkull)
                    remainingSkulls.Add(pickup);
            }
        }
    }

    public void RegisterLevelSkullCollected(Pickup skull)
    {
        remainingSkulls.Remove(skull);
    }

    public void RespawnPlayer()
    {
        if (isRespawning || isGameOver || isEnding) return;
        CurrentLives = Mathf.Max(0, CurrentLives - 1);
        PlayerPrefs.SetInt("PlayerLives", CurrentLives);
        if (CurrentLives == 0)
        {
            isGameOver = true;
            PlayerController.Instance.stopInput = true;
            PlayerController.Instance.gameObject.SetActive(false);
            if (UITKController.instance != null) UITKController.instance.ShowGameOver(true);
            return;
        }
        isRespawning = true;
        StartCoroutine(RespawnCo());
    }

    private IEnumerator RespawnCo()
    {
        PlayerController.Instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitToRespawn);
        PlayerController.Instance.transform.position = CheckpointController.Instance.CurrentSpawnPoint;
        PlayerController.Instance.gameObject.SetActive(true);
        isRespawning = false;
    }

    public void EndLevel()
    {
        if (!CanEndLevel || isEnding || isGameOver || isRespawning) return;
        isEnding = true;
        StartCoroutine(EndLevelCo());
    }

    private IEnumerator EndLevelCo()
    {
        PlayerController.Instance.stopInput = true;
        // Logic for transitioning to result screen could go here
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}