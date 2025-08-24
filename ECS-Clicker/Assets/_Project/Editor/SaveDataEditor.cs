// Filename: SaveDataEditor.cs
// Location: _Project/Scripts/Editor/
using UnityEditor;
using UnityEngine;

public class SaveDataEditor
{
    // This creates a new menu item in the Unity Editor under "Tools > Clear Save Data"
    [MenuItem("Tools/Clear Save Data")]
    private static void ClearSaveData()
    {
        // This deletes ALL data stored in PlayerPrefs.
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("<b><color=orange>All PlayerPrefs save data has been cleared.</color></b>");
    }
}