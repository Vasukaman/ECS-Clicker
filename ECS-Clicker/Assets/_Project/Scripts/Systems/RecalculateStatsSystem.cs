using Leopotam.EcsLite;

/// <summary>
/// Reacts to RecalculateStatsRequest events to update a business's derived data (income and costs).
/// This system is the single source of truth for all business-related math.
/// </summary>
public class RecalculateStatsSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        EcsFilter requestFilter = world.Filter<RecalculateStatsRequest>().End();
        EcsPool<RecalculateStatsRequest> requestPool = world.GetPool<RecalculateStatsRequest>();

        foreach (int requestEntity in requestFilter)
        {
            ref RecalculateStatsRequest request = ref requestPool.Get(requestEntity);
            ProcessRequest(systems, world, ref request);

            world.DelEntity(requestEntity);
        }
    }


    private void ProcessRequest(EcsSystems systems, EcsWorld world, ref RecalculateStatsRequest request)
    {
        if (request.TargetBusiness.Unpack(world, out int businessEntity))
        {
            EcsPool<BusinessComponent> businessPool = world.GetPool<BusinessComponent>();
            ref BusinessComponent business = ref businessPool.Get(businessEntity);

            GameConfig gameConfig = systems.GetShared<SharedData>().GameConfig;
            BusinessConfig config = gameConfig.Businesses[business.ConfigId];

            business.CurrentIncome = CalculateFinalIncome(ref business, config);
            business.LevelUpCost = CalculateNextLevelUpCost(ref business, config);
        }
    }

    private double CalculateFinalIncome(ref BusinessComponent business, BusinessConfig config)
    {
        if (business.Level <= 0) return 0;

        double incomeMultiplier = 1.0;
        if (business.IsUpgrade1Purchased)
            incomeMultiplier += config.Upgrade1.IncomeMultiplierPercent / 100.0;
        if (business.IsUpgrade2Purchased)
            incomeMultiplier += config.Upgrade2.IncomeMultiplierPercent / 100.0;

        return business.Level * config.BaseIncome * incomeMultiplier;
    }


    private double CalculateNextLevelUpCost(ref BusinessComponent business, BusinessConfig config)
    {
        return (business.Level + 1) * config.BaseCost;
    }
}