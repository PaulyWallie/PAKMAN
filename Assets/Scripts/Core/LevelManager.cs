using System.Collections;
using UnityEngine;

/// <summary>
/// Responsible ONLY for managing high-level level flow (Respawning, Ending).
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToRespawn = 2f;

    private void Awake()
    {
        instance = this;
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
    }
}



