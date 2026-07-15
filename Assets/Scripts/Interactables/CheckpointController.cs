using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController Instance { get; private set; }

    public Vector3 CurrentSpawnPoint { get; private set; }
    private Checkpoint _activeCheckpoint;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable() => Checkpoint.OnCheckpointActivated += HandleCheckpointActivated;
    private void OnDisable() => Checkpoint.OnCheckpointActivated -= HandleCheckpointActivated;

    private void HandleCheckpointActivated(Checkpoint newCheckpoint)
    {
        // Deactivate the old one if it exists
        if (_activeCheckpoint != null && _activeCheckpoint != newCheckpoint)
        {
            _activeCheckpoint.Deactivate();
        }

        _activeCheckpoint = newCheckpoint;
        CurrentSpawnPoint = newCheckpoint.transform.position;

        Debug.Log($"Spawn Point updated to: {CurrentSpawnPoint}");
    }
}