// Filename: Events.cs
using Leopotam.EcsLite;

/// <summary>
/// A marker interface for any component that is a short-lived, broadcast-style event.
/// </summary>
public interface IBroadcastEvent { }

// --- Broadcast Events (Announcements) ---

/// <summary>
/// A broadcast event fired when a business has collected revenue.
/// </summary>
public struct RevenueCollectedEvent : IBroadcastEvent
{
    public double Amount;
    public EcsPackedEntity SourceBusiness;
}

// --- Request Events (Commands) ---

/// <summary>
/// A request to level up a specific business.
/// </summary>
public struct LevelUpRequest
{
    public EcsPackedEntity TargetBusiness;
}

/// <summary>
/// A request to purchase an upgrade for a specific business.
/// </summary>
public struct UpgradeRequest
{
    public EcsPackedEntity TargetBusiness;
    public int UpgradeId; // 0 for the first upgrade, 1 for the second
}

/// <summary>
/// A request to recalculate the derived stats (income, costs) for a business.
/// </summary>
public struct RecalculateStatsRequest
{
    public EcsPackedEntity TargetBusiness;
}