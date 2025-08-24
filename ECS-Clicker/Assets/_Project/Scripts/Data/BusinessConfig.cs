// Filename: BusinessConfig.cs
// Location: _Project/Scripts/Data/
using UnityEngine;

[System.Serializable]
public struct UpgradeConfig
{
    public string Name;
    public double Price;
    public float IncomeMultiplierPercent;
}


[CreateAssetMenu(fileName = "NewBusinessConfig", menuName = "Clicker/Business Config")]
public class BusinessConfig : ScriptableObject
{
    // We removed the 'Id' field as it's no longer needed
    public string BusinessName;
    public float IncomeDelay;
    public double BaseCost;
    public double BaseIncome;

    // Add these two fields for the upgrades
    public UpgradeConfig Upgrade1;
    public UpgradeConfig Upgrade2;
}