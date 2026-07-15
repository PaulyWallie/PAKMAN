using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // A static event that any script can subscribe to
    public static event Action<Checkpoint> OnCheckpointActivated;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    private bool _isActive;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isActive)
        {
            Activate();
        }
    }

    public void Activate()
    {
        _isActive = true;
        sr.sprite = activeSprite;
        // Notify the system that THIS checkpoint is now the active one
        OnCheckpointActivated?.Invoke(this);
    }

    public void Deactivate()
    {
        _isActive = false;
        sr.sprite = inactiveSprite;
    }
}