using Leopotam.EcsLite;

public class BusinessRevenueSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        var world = systems.GetWorld();
        var gameConfig = systems.GetShared<SharedData>().GameConfig;

        var filter = world.Filter<BusinessComponent>().End();
        var businessPool = world.GetPool<BusinessComponent>();
        var eventPool = world.GetPool<RevenueCollectedEvent>();

        foreach (var entity in filter)
        {
            ref var business = ref businessPool.Get(entity);
            var config = gameConfig.Businesses[business.ConfigId];

            if (business.Level > 0 && business.IncomeTimer >= config.IncomeDelay)
            {
                business.IncomeTimer -= config.IncomeDelay;

                double finalIncome = business.CurrentIncome;

                // Create the event with that value.
                var eventEntity = world.NewEntity();
                ref var evt = ref eventPool.Add(eventEntity);
                evt.Amount = finalIncome;
            }
        }
    }
}