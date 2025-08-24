// Filename: Components.cs
// Location: _Project/Scripts/Components/

// A "tag" component to identify the single player entity
using Leopotam.EcsLite;

struct PlayerTag { }

// Holds the player's money
struct BalanceComponent
{
    public double Value;
}

// Holds the state of a single business
struct BusinessComponent
{
    public int ConfigId; // Changed to int for performance (array index)
    public int Level;
    public float IncomeTimer; // Tracks seconds passed

    // Add these fields to store calculated data
    public double CurrentIncome;
    public double LevelUpCost;

    public bool IsUpgrade1Purchased;
    public bool IsUpgrade2Purchased;
}

public struct RecalculateStatsRequest
{
    public EcsPackedEntity TargetBusiness;
}

// Links an entity to its MonoBehaviour view in the scene
struct ViewComponent
{
    public BusinessView Value;
}

// Add to Components.cs
public struct RevenueCollectedEvent
{
    public double Amount;
}


// Add to Components.cs

// An event created when the player clicks the "Level Up" button
public struct LevelUpRequest
{
    // Which business entity should be leveled up?
    public EcsPackedEntity TargetBusiness;
}

// An event created when the player clicks an "Upgrade" button
public struct UpgradeRequest
{
    public EcsPackedEntity TargetBusiness;
    public int UpgradeId; // 0 for the first upgrade, 1 for the second
}