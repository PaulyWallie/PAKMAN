using UnityEngine;
using System.Collections;

/// <summary>
/// Handles invincibility frames and visual feedback (flickering).
/// </summary>
public class DamageBuffer : MonoBehaviour
{
    [Header("Invincibility Settings")]
    public float invincibilityDuration = 1f;
    public float flickerInterval = 0.1f;

    public bool IsInvincible { get; private set; }

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartInvincibility()
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        IsInvincible = true;
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        IsInvincible = false;
    }
}
