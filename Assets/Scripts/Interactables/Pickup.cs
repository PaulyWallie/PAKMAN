using UnityEngine;

public enum PickupType
{
    Coin = 0,
    Health = 1,
    LevelSkull = 2,
    UpgradeSkull = 3,
    ExtraLife = 4
}

public class Pickup : MonoBehaviour
{
    public PickupType pickupType;
    public int amount = 1;
    public GameObject pickupEffect;

    [SerializeField, Tooltip("Unique across the game for each placed UpgradeSkull.")]
    private string upgradeSkullId;
    private bool collected;

    private void Start()
    {
        if (pickupType != PickupType.UpgradeSkull) return;
        if (string.IsNullOrWhiteSpace(upgradeSkullId))
        {
            Debug.LogError("UpgradeSkull requires a unique collectible ID.", this);
            gameObject.SetActive(false);
        }
        else if (PlayerProgress.IsCollected(upgradeSkullId)) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) Collect();
    }

    private void Collect()
    {
        if (collected || !TryApplyReward(out SoundType sound)) return;
        collected = true;
        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(sound);
        if (pickupEffect != null) Instantiate(pickupEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private bool TryApplyReward(out SoundType sound)
    {
        sound = default;
        if (amount <= 0) return false;
        switch (pickupType)
        {
            case PickupType.Coin:
                if (LevelStats.instance == null) return false;
                LevelStats.instance.AddCoin(amount);
                sound = SoundType.CoinPickup;
                break;
            case PickupType.Health:
                if (PlayerHealthController.instance == null) return false;
                PlayerHealthController.instance.Heal(amount);
                sound = SoundType.HealthPickup;
                break;
            case PickupType.ExtraLife:
                if (LevelManager.Instance == null) return false;
                LevelManager.Instance.AddLives(amount);
                sound = SoundType.ExtraLife;
                break;
            case PickupType.LevelSkull:
                if (LevelManager.Instance == null) return false;
                LevelManager.Instance.RegisterLevelSkullCollected(this);
                if (LevelStats.instance != null) LevelStats.instance.AddSkull(amount);
                sound = SoundType.SkullPickup;
                break;
            case PickupType.UpgradeSkull:
                if (!PlayerProgress.CollectUpgradeSkull(upgradeSkullId)) return false;
                sound = SoundType.Powerup;
                break;
            default:
                return false;
        }
        return true;
    }
}
