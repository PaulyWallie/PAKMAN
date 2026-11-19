using System.Collections.Generic;
using UnityEngine;

public static class GameSaveReset
{
    public const string LivesKey = "PlayerLives";
    public const string CoinsKey = "CoinCount";
    public const string InitialLevelKey = "Level1";

    public static void ResetUpgradesOnly() => PlayerProgress.ResetUpgrades();

    public static void ResetGameSave(IEnumerable<string> levelKeys)
    {
        PlayerProgress.ResetUpgrades(false);
        PlayerPrefs.DeleteKey(LivesKey);
        PlayerPrefs.DeleteKey(CoinsKey);
        PlayerPrefs.DeleteKey(InitialLevelKey);
        if (levelKeys != null)
            foreach (string key in levelKeys)
                if (!string.IsNullOrWhiteSpace(key)) PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
    }

    public static void StartNewGame(IEnumerable<string> levelKeys)
    {
        ResetGameSave(levelKeys);
        LevelManager.ResetLives();
        PlayerPrefs.SetInt(CoinsKey, 0);
        PlayerPrefs.Save();
    }
}
