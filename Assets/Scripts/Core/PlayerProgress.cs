using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerProgressData
{
    public int upgradeSkulls;
    public List<string> collectedUpgradeSkullIds = new List<string>();
}

public static class PlayerProgress
{
    private const string SaveKey = "PlayerProgress";
    private static PlayerProgressData _data;

    private static PlayerProgressData Data
    {
        get
        {
            if (_data == null)
            {
                try { _data = JsonUtility.FromJson<PlayerProgressData>(PlayerPrefs.GetString(SaveKey, "{}")); }
                catch (ArgumentException) { Debug.LogWarning("Invalid player progress save; using empty progress."); }
                if (_data == null) _data = new PlayerProgressData();
                if (_data.collectedUpgradeSkullIds == null) _data.collectedUpgradeSkullIds = new List<string>();
            }
            return _data;
        }
    }

    public static int UpgradeSkulls => Data.upgradeSkulls;
    public static bool DoubleJumpUnlocked => UpgradeSkulls >= 1;

    public static event Action AbilityUnlocked;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ClearCache()
    {
        _data = null;
        AbilityUnlocked = null;
    }

    public static void ResetUpgrades(bool save = true)
    {
        PlayerPrefs.DeleteKey(SaveKey);
        _data = null;
        if (save) PlayerPrefs.Save();
    }

    public static bool IsCollected(string id) => Data.collectedUpgradeSkullIds.Contains(id);

    public static bool CollectUpgradeSkull(string id)
    {
        if (string.IsNullOrWhiteSpace(id) || IsCollected(id)) return false;
        bool wasUnlocked = DoubleJumpUnlocked;
        Data.collectedUpgradeSkullIds.Add(id);
        Data.upgradeSkulls++;
        Save();
        if (!wasUnlocked && DoubleJumpUnlocked) AbilityUnlocked?.Invoke();
        return true;
    }

    public static void ResetForNewGame()
    {
        _data = new PlayerProgressData();
        Save();
    }

    private static void Save()
    {
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(Data));
        PlayerPrefs.Save();
    }
}
