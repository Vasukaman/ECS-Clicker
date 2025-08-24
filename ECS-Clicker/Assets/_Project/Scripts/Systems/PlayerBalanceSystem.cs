// Filename: PlayerBalanceSystem.cs
// Location: _Project/Scripts/ECS/Systems/
using Leopotam.EcsLite;

public class PlayerBalanceSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        var world = systems.GetWorld();

        var eventFilter = world.Filter<RevenueCollectedEvent>().End();
        var playerFilter = world.Filter<PlayerTag>().Inc<BalanceComponent>().End();

        if (eventFilter.GetEntitiesCount() > 0)
        {
            var eventPool = world.GetPool<RevenueCollectedEvent>();
            var balancePool = world.GetPool<BalanceComponent>();

            // This is a rare case where we know there's only one player entity
            // In a real game, you might need a more robust way to find the player
            var playerEntity = playerFilter.GetRawEntities()[0];
            ref var playerBalance = ref balancePool.Get(playerEntity);

            foreach (var eventEntity in eventFilter)
            {
                ref var evt = ref eventPool.Get(eventEntity);
                playerBalance.Value += evt.Amount;
            }
        }
    }
}