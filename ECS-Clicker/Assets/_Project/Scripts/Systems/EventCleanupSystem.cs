// Filename: EventCleanupSystem.cs
// Location: _Project/Scripts/ECS/Systems/
using Leopotam.EcsLite;

public class EventCleanupSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<RevenueCollectedEvent>().End();
        foreach (var entity in filter)
        {
            world.DelEntity(entity);
        }
    }
}