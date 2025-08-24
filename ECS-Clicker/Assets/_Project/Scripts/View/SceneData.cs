// Filename: SceneData.cs
// Location: _Project/Scripts/Views/
using TMPro;
using UnityEngine;

// This is a simple "bag" for all the UI references in the scene.
public class SceneData : MonoBehaviour
{
    public TMP_Text BalanceText;
    public BusinessView[] BusinessViews;

    public BusinessView BusinessViewPrefab;
    public RectTransform BusinessPanelContainer; // T
}

