using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController instance;
    [Header("Checkpoint Settings")] Checkpoint[] checkpoints;
    public Vector2 spawnPoint;

    private void Awake()
    {
        if (instance)
            instance = this;
        else if (instance != this)
        {
            //Debug.LogWarning("There must be one instance of CheckpointController");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        if (PlayerController.instance)
            spawnPoint = PlayerController.instance.transform.position;
        else
            Debug.LogError("PlayerController instance is null");
    }

    public void DeactivateCheckpoints()
    {
        if (checkpoints.Length > 0)
        {
            foreach (Checkpoint checkpoint in checkpoints)
                checkpoint.ResetCheckpoint();
        }
        else
            Debug.LogError("There are no checkpoints to deactivate");
    }

    public void SetSpawnPoint(Vector2 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
        Debug.Log($"spawn pont updated to {spawnPoint}");
    }
}