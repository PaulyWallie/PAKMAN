using UnityEngine;

/// <summary>
/// Responsible ONLY for triggering player-specific sound effects.
/// </summary>
public class PlayerAudioHandler : MonoBehaviour
{
    public AudioSource jumpSFX;
    public AudioSource hurtSFX;

    private void Awake()
    {
        // Auto-assign if missing on prefab/instance
        if (jumpSFX == null || hurtSFX == null)
        {
            var sources = GetComponents<AudioSource>();
            foreach (var s in sources)
            {
                if (s.generator != null)
                {
                    // Basic heuristic: check generator asset name via string comparison
                    string genName = s.generator.ToString();
                    if (genName.Contains("Jump")) jumpSFX = s;
                    if (genName.Contains("Hurt")) hurtSFX = s;
                }
            }
        }
    }

    public void PlayJump()
    {
        if (jumpSFX != null) jumpSFX.Play();
    }

    public void PlayHurt()
    {
        if (hurtSFX != null) hurtSFX.Play();
    }
}
