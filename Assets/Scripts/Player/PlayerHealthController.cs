using System;
using UnityEngine;

/// <summary>
/// Bridge component for player health.
/// </summary>
public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public static event Action<int, int> OnHealthChanged;

    public int currentHealth => health != null ? health.currentHealth : 0;
    public int maxHealth => health != null ? health.maxHealth : 0;

    private Health health;

    void Awake()
    {
        instance = this;
        health = GetComponent<Health>();
    }

    void OnEnable()
    {
        if (health != null)
        {
            health.OnHealthChanged += HandleHealthChanged;
            health.OnDeath += HandleDeath;
        }
    }

    void OnDisable()
    {
        if (health != null)
        {
            health.OnHealthChanged -= HandleHealthChanged;
            health.OnDeath -= HandleDeath;
        }
    }

    private void HandleHealthChanged(int current, int max)
    {
        OnHealthChanged?.Invoke(current, max);
    }

    private void HandleDeath()
    {
        LevelManager.instance.RespawnPlayer();
        health.Heal(health.maxHealth);
    }

    public void TakeDamage(int halfHearts = 1)
    {
        health.TakeDamage(halfHearts);
    }

    public void Heal(int halfHearts = 2)
    {
        health.Heal(halfHearts);
    }

    public void AddMaxHeart()
    {
        health.AddMaxHealth(2, true);
    }
}
