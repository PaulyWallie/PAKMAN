using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

internal static class GameSaveResetMenu
{
    private const string UpgradeMenu = "PAKMAN/Save Data/Reset Upgrades Only";
    private const string GameMenu = "PAKMAN/Save Data/Reset Game Save";

    [MenuItem(UpgradeMenu)]
    private static void ResetUpgrades()
    {
        if (!CanReset()) return;
        if (!EditorUtility.DisplayDialog("Reset upgrades?", "Clear collected upgrade skulls and unlocked abilities? Lives, coins and level progress will be kept. Changes take effect on your next Play session.", "Reset upgrades", "Cancel")) return;
        GameSaveReset.ResetUpgradesOnly();
        Debug.Log("Upgrade save reset. Enter Play Mode to collect upgrade skulls again.");
    }

    [MenuItem(GameMenu)]
    private static void ResetGame()
    {
        if (!CanReset()) return;
        string[] keys = GetLevelKeys();
        if (!EditorUtility.DisplayDialog("Reset game save?", "Clear this game's upgrades, lives, coins and level progress? The next game starts with 3 lives. Unrelated preferences will be kept.", "Reset game save", "Cancel")) return;
        GameSaveReset.ResetGameSave(keys);
        Debug.Log("Game save reset. Next Play session starts with 3 lives and locked upgrades.");
    }

    [MenuItem(UpgradeMenu, true)]
    [MenuItem(GameMenu, true)]
    private static bool CanReset() => !EditorApplication.isPlayingOrWillChangePlaymode;

    // Read actual serialized MainMenu level lists without opening or modifying scenes.
    internal static string[] GetLevelKeys()
    {
        var keys = new HashSet<string>();
        foreach (string guid in AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes" }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string text = File.ReadAllText(path);
            foreach (Match block in Regex.Matches(text, @"(?m)^  levelNames:\s*\r?\n((?:  - [^\r\n]*\r?\n)+)"))
                foreach (Match entry in Regex.Matches(block.Groups[1].Value, @"(?m)^  - (.+)$"))
                {
                    string key = entry.Groups[1].Value.Trim().Trim('"', '\'');
                    if (!string.IsNullOrWhiteSpace(key)) keys.Add(key);
                }
        }
        return new List<string>(keys).ToArray();
    }
}
