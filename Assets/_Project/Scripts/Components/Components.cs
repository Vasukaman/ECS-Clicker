using Leopotam.EcsLite;

/// <summary>
/// A tag to identify the single player entity.
/// </summary>
public struct PlayerTag { }

/// <summary>
/// Holds the player's current money.
/// </summary>
public struct BalanceComponent
{
    public double Value;
}

/// <summary>
/// Holds the dynamic state of a single business.
/// </summary>
public struct BusinessComponent
{
    public int ConfigId;
    public int Level;
    public float IncomeTimer;
    public bool IsUpgrade1Purchased;
    public bool IsUpgrade2Purchased;
    public double CurrentIncome;
    public double LevelUpCost;
}

/// <summary>
/// Links an ECS entity to its MonoBehaviour view in the scene.
/// </summary>
public struct ViewComponent
{
    public BusinessView Value;
}