using UnityEngine;

/// <summary>
/// Responsible ONLY for tracking level-specific statistics like coins and time.
/// </summary>
public class LevelStats : MonoBehaviour
{
    public static LevelStats instance;

    [Header("Stats")]
    public int coinsCollected;
    public int skullsCollected;
    public float timeInLevel;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        timeInLevel += Time.deltaTime;
    }

    public void AddCoin(int amount) => coinsCollected += amount;
    public void AddSkull(int amount) => skullsCollected += amount;
}
