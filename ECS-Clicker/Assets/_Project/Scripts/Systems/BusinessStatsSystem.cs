// Filename: BusinessStatsSystem.cs
// Location: _Project/Scripts/ECS/Systems/
using Leopotam.EcsLite;

public class BusinessStatsSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        var world = systems.GetWorld();
        var gameConfig = systems.GetShared<SharedData>().GameConfig;

        // Filter for any entity that has a "RecalculateStatsRequest" component
        var filter = world.Filter<RecalculateStatsRequest>().End();

        // Get the pools for the components we'll be working with
        var requestPool = world.GetPool<RecalculateStatsRequest>();
        var businessPool = world.GetPool<BusinessComponent>();

        foreach (var requestEntity in filter)
        {
            ref var request = ref requestPool.Get(requestEntity);

            // Safely unpack the entity reference from the request
            if (request.TargetBusiness.Unpack(world, out int businessEntity))
            {
                ref var business = ref businessPool.Get(businessEntity);
                var config = gameConfig.Businesses[business.ConfigId];

                // --- CENTRALIZED CALCULATION LOGIC ---

                // 1. Calculate the final income
                double incomeMultiplier = 1.0;
                if (business.IsUpgrade1Purchased)
                {
                    incomeMultiplier += config.Upgrade1.IncomeMultiplierPercent / 100.0;
                }
                if (business.IsUpgrade2Purchased)
                {
                    incomeMultiplier += config.Upgrade2.IncomeMultiplierPercent / 100.0;
                }

                // Only calculate income if the business is owned
                if (business.Level > 0)
                {
                    business.CurrentIncome = business.Level * config.BaseIncome * incomeMultiplier;
                }
                else
                {
                    business.CurrentIncome = 0;
                }

                // 2. Calculate the cost for the NEXT level up
                business.LevelUpCost = (business.Level + 1) * config.BaseCost;

                // --- END OF LOGIC ---
            }

            // The request has been processed, so we delete its entity
            world.DelEntity(requestEntity);
        }
    }
}