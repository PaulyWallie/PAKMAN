using UnityEngine;

/// <summary>
/// Deals damage to the player on contact. 
/// Works with both triggers and solid collisions.
/// </summary>
public class DamagePlayer : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("How much damage to deal (in half-hearts).")]
    public int damageAmount = 1;

    [Tooltip("If true, the player is respawned instantly (ignoring health).")]
    public bool killInstantly = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            HandleDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandleDamage();
        }
    }

    private void HandleDamage()
    {
        if (killInstantly)
        {
            LevelManager.instance.RespawnPlayer();
        }
        else
        {
            PlayerHealthController.instance.TakeDamage(damageAmount);
            PlayerController.instance.KnockBack(transform.position);
        }
    }
}
