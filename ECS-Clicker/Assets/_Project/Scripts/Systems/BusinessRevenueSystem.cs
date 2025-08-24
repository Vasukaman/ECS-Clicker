using Leopotam.EcsLite;

/// <summary>
/// Checks business timers and creates a RevenueCollectedEvent when a payout is ready.
/// This system acts as a producer of revenue events.
/// </summary>
public class BusinessRevenueSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        EcsFilter businessFilter = world.Filter<BusinessComponent>().End();

        foreach (int businessEntity in businessFilter)
        {
            ProcessBusinessPayout(systems, world, businessEntity);
        }
    }

    private void ProcessBusinessPayout(EcsSystems systems, EcsWorld world, int businessEntity)
    {
        EcsPool<BusinessComponent> businessPool = world.GetPool<BusinessComponent>();
        ref BusinessComponent business = ref businessPool.Get(businessEntity);

        if (IsPayoutReady(systems, ref business))
        {
            GameConfig gameConfig = systems.GetShared<SharedData>().GameConfig;
            BusinessConfig config = gameConfig.Businesses[business.ConfigId];
            business.IncomeTimer -= config.IncomeDelay;

            CreatePayoutEvent(world, business.CurrentIncome);
        }
    }

    private bool IsPayoutReady(EcsSystems systems, ref BusinessComponent business)
    {
        if (business.Level <= 0) return false;

        GameConfig gameConfig = systems.GetShared<SharedData>().GameConfig;
        BusinessConfig config = gameConfig.Businesses[business.ConfigId];

        return business.IncomeTimer >= config.IncomeDelay;
    }

    private void CreatePayoutEvent(EcsWorld world, double amount)
    {
        int eventEntity = world.NewEntity();
        ref RevenueCollectedEvent evt = ref world.GetPool<RevenueCollectedEvent>().Add(eventEntity);
        evt.Amount = amount;
    }
}