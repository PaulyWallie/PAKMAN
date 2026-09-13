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

    private void Awake()
    {
        Instance = this;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }

    private IEnumerator RespawnCo()
    {
        PlayerController.Instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitToRespawn);
        PlayerController.Instance.transform.position = CheckpointController.Instance.CurrentSpawnPoint;
        PlayerController.Instance.gameObject.SetActive(true);
    }

    public void EndLevel()
    {
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



