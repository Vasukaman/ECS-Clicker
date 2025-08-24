// Filename: SaveDataEditor.cs
// Location: _Project/Scripts/Editor/
using UnityEditor;
using UnityEngine;
using System.Linq; // Required for .FirstOrDefault()

public class SaveDataEditor
{
    // --- Main Control ---

    [MenuItem("Tools/Save Data/Clear All Save Data")]
    private static void ClearAllSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("<b><color=orange>All PlayerPrefs save data has been cleared.</color></b>");
    }

    // --- Cheats ---

    [MenuItem("Tools/Save Data/Cheats/Give Max Money & Reset Businesses")]
    private static void ApplyCheatSave()
    {
        GameConfig gameConfig = GetGameConfig();
        if (gameConfig == null) return;

        // Give a huge amount of money
        PlayerPrefs.SetString("Player_Balance", "999999999");

        // Reset all businesses to their default starting state
        ResetBusinessProgress(gameConfig);

        PlayerPrefs.Save();
        Debug.Log("<b><color=green>Cheat Applied: Max money and reset businesses.</color></b>");
    }

    // --- Resets ---

    [MenuItem("Tools/Save Data/Resets/Reset Player Balance Only")]
    private static void ResetPlayerBalance()
    {
        GameConfig gameConfig = GetGameConfig();
        if (gameConfig == null) return;

        PlayerPrefs.SetString("Player_Balance", gameConfig.StartingBalance.ToString());
        PlayerPrefs.Save();
        Debug.Log("<b><color=yellow>Player balance has been reset to default.</color></b>");
    }

    [MenuItem("Tools/Save Data/Resets/Reset Business Progress Only")]
    private static void ResetBusinessProgress()
    {
        GameConfig gameConfig = GetGameConfig();
        if (gameConfig == null) return;

        // We call our internal helper method
        ResetBusinessProgress(gameConfig);

        PlayerPrefs.Save();
        Debug.Log("<b><color=yellow>All business progress has been reset to default.</color></b>");
    }

    // --- Helper Methods ---

    private static void ResetBusinessProgress(GameConfig gameConfig)
    {
        foreach (BusinessConfig businessConfig in gameConfig.Businesses)
        {
            string id = businessConfig.BusinessId;
            PlayerPrefs.DeleteKey($"{id}_Level");
            PlayerPrefs.DeleteKey($"{id}_IncomeTimer");
            PlayerPrefs.DeleteKey($"{id}_Upgrade1");
            PlayerPrefs.DeleteKey($"{id}_Upgrade2");
        }
    }

    /// <summary>
    /// A robust way to find the GameConfig asset anywhere in the project.
    /// </summary>
    private static GameConfig GetGameConfig()
    {
        // Find all assets of type GameConfig
        string[] guids = AssetDatabase.FindAssets("t:GameConfig");
        if (guids.Length == 0)
        {
            Debug.LogError("Could not find GameConfig asset in the project.");
            return null;
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<GameConfig>(path);
    }
}