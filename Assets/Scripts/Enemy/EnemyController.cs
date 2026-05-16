using UnityEngine;

/// <summary>
/// Base class for all enemies. Handles modular health integration and stomp detection.
/// </summary>
[RequireComponent(typeof(Collider2D), typeof(Health))]
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int contactDamage = 1;

    protected bool isDead;
    protected Collider2D enemyCollider;
    protected Health health;

    protected virtual void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        health = GetComponent<Health>();
    }

    protected virtual void OnEnable()
    {
        if (health != null)
        {
            health.OnDeath += Die;
        }
    }

    protected virtual void OnDisable()
    {
        if (health != null)
        {
            health.OnDeath -= Die;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        if (!collision.collider.CompareTag("Player")) return;

        // Skip damage if the hit came from above (Player handles stomp)
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                return;
            }
        }

        DamagePlayer();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        health.TakeDamage(damage);
    }

    private void DamagePlayer()
    {
        if (PlayerHealthController.instance != null)
            PlayerHealthController.instance.TakeDamage(contactDamage);
        
        if (PlayerController.instance != null)
            PlayerController.instance.KnockBack(transform.position);
    }

    protected virtual void Die()
    {
        isDead = true;
        OnDeath();
        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(SoundType.Explosion);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the enemy dies. Can be overridden for particles/drops.
    /// </summary>
    protected virtual void OnDeath() { }
}
