using System;
using UnityEngine;

/// <summary>
/// Generic health management. Works with DamageBuffer if present.
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 2;
    public int currentHealth;

    public event Action<int, int> OnHealthChanged; // current, max
    public event Action OnDamaged;
    public event Action OnDeath;

    private DamageBuffer damageBuffer;

    private void Awake()
    {
        currentHealth = maxHealth;
        damageBuffer = GetComponent<DamageBuffer>();
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (damageBuffer != null && damageBuffer.IsInvincible)
            return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        OnDamaged?.Invoke();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (damageBuffer != null && currentHealth > 0)
        {
            damageBuffer.StartInvincibility();
        }

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void AddMaxHealth(int amount, bool fillNewHealth = true)
    {
        maxHealth += amount;
        if (fillNewHealth)
        {
            currentHealth += amount;
        }
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
