using Leopotam.EcsLite;

/// <summary>
/// Reacts to RevenueCollectedEvent and adds the amount to the player's balance.
/// </summary>
public class PlayerBalanceSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        EcsFilter eventFilter = world.Filter<RevenueCollectedEvent>().End();
        EcsFilter playerFilter = world.Filter<PlayerTag>().Inc<BalanceComponent>().End();

        // Early exit if there are no events to process or no player entity exists.
        if (eventFilter.GetEntitiesCount() == 0 || playerFilter.GetEntitiesCount() == 0)
        {
            return;
        }

        double totalRevenue = GetTotalRevenueFromEvents(world, eventFilter);

        if (totalRevenue > 0)
        {
            AddRevenueToPlayer(world, playerFilter, totalRevenue);
        }
    }

    private void AddRevenueToPlayer(EcsWorld world, EcsFilter playerFilter, double totalRevenue)
    {
        ref BalanceComponent playerBalance = ref GetPlayerBalance(world, playerFilter);
        playerBalance.Value += totalRevenue;
    }

    private double GetTotalRevenueFromEvents(EcsWorld world, EcsFilter eventFilter)
    {
        EcsPool<RevenueCollectedEvent> eventPool = world.GetPool<RevenueCollectedEvent>();
        double totalAmount = 0;

        foreach (int eventEntity in eventFilter)
        {
            ref RevenueCollectedEvent evt = ref eventPool.Get(eventEntity);
            totalAmount += evt.Amount;
        }

        return totalAmount;
    }

    /// <summary>
    /// A safe way to get a reference to the single player's BalanceComponent.
    /// Assumes the filter has already been checked to not be empty.
    /// </summary>
    private ref BalanceComponent GetPlayerBalance(EcsWorld world, EcsFilter playerFilter)
    {
  
        int playerEntity = playerFilter.GetRawEntities()[0];
        EcsPool<BalanceComponent> balancePool = world.GetPool<BalanceComponent>();
        return ref balancePool.Get(playerEntity);
    }
}