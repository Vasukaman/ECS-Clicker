using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data container that holds all display text for a single business,
/// linking it via a BusinessId.
/// </summary>
[System.Serializable]
public struct BusinessTextData
{
    public string BusinessId;
    public string BusinessName;
    public string Upgrade1Name;
    public string Upgrade2Name;
}

/// <summary>
/// ScriptableObject that contains all localizable text for the game.
/// </summary>
[CreateAssetMenu(fileName = "NamesConfig", menuName = "Game/Names Config")]
public class NamesConfig : ScriptableObject
{
    public List<BusinessTextData> AllBusinessTexts;
}