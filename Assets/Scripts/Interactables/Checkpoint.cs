using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Sprites")]
    public SpriteRenderer sr;
    public Sprite cpOn, cpOff;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (sr.sprite != cpOn)
            {
                CheckpointController.instance.DeactivateCheckpoints();
                ActivateCheckpoint();
                CheckpointController.instance.SetSpawnPoint(transform.position);
                if (AudioManager.instance != null) AudioManager.instance.PlaySFX(SoundType.Powerup);
            }
        }
        else
            Debug.LogError("CheckpointController not found");
    }

    public void ResetCheckpoint()
    {
        if(sr)
            sr.sprite = cpOff;
        else
            Debug.LogError("SpriteRenderer not found");
    }

    public void ActivateCheckpoint()
    {
        if (sr)
            sr.sprite = cpOn;
        else
            Debug.LogError("SpriteRenderer not found");
    }
}