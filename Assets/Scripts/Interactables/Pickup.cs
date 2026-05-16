using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupType pickupType;
    public int amount = 1;
    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        switch (pickupType)
        {
            case PickupType.Coin:
                if (LevelStats.instance != null) LevelStats.instance.AddCoin(amount);
                if (AudioManager.instance != null) AudioManager.instance.PlaySFX(SoundType.CoinPickup);
                break;

            case PickupType.Health:
                PlayerHealthController.instance.Heal(amount);
                if (AudioManager.instance != null) AudioManager.instance.PlaySFX(SoundType.HealthPickup);
                break;

            case PickupType.Skull:
                if (LevelStats.instance != null) LevelStats.instance.AddSkull(amount);
                if (AudioManager.instance != null) AudioManager.instance.PlaySFX(SoundType.SkullPickup);
                break;
        }

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject);
    }
}