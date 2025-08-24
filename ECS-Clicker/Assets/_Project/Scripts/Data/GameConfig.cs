// Filename: GameConfig.cs
// Location: _Project/Scripts/Data/
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Clicker/Game Config")]
public class GameConfig : ScriptableObject
{
    public List<BusinessConfig> Businesses;
    public double StartingBalance = 0;
}