using Leopotam.EcsLite;

/// <summary>
/// Cleans up all short-lived broadcast event entities at the end of the frame.
/// </summary>
public class EventCleanupSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();

        CleanupEvents<RevenueCollectedEvent>(world);
    }

    /// <summary>
    /// A generic helper method that finds and deletes all entities with a specific event component.
    /// </summary>
    private void CleanupEvents<T>(EcsWorld world) where T : struct, IBroadcastEvent
    {
        EcsFilter eventFilter = world.Filter<T>().End();
        foreach (int entity in eventFilter)
        {
            world.DelEntity(entity);
        }
    }
}