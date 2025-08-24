// Filename: LevelUpSystem.cs
// Location: _Project/Scripts/ECS/Systems/
using Leopotam.EcsLite;

public class LevelUpSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        EcsFilter requestFilter = world.Filter<LevelUpRequest>().End();

        foreach (int requestEntity in requestFilter)
        {
            ProcessLevelUpRequest(systems, world, requestEntity);
            world.DelEntity(requestEntity);
        }
    }

    private void ProcessLevelUpRequest(EcsSystems systems, EcsWorld world, int requestEntity)
    {
        ref LevelUpRequest request = ref world.GetPool<LevelUpRequest>().Get(requestEntity);

        if (request.TargetBusiness.Unpack(world, out int businessEntity))
        {
            ref BusinessComponent business = ref world.GetPool<BusinessComponent>().Get(businessEntity);
            ref BalanceComponent playerBalance = ref GetPlayerBalance(world);

            if (CanAffordLevelUp(ref business, ref playerBalance))
            {
                ApplyLevelUp(world, ref playerBalance, ref business, ref request);
            }
        }
    }

    private bool CanAffordLevelUp(ref BusinessComponent business, ref BalanceComponent playerBalance)
    {
        // A business must be purchased (level > 0) to be leveled up further,
        // or have a level of 0 to be bought for the first time.
        return playerBalance.Value >= business.LevelUpCost;
    }

    private void ApplyLevelUp(EcsWorld world, ref BalanceComponent playerBalance, ref BusinessComponent business, ref LevelUpRequest request)
    {
        // 1. Deduct cost
        playerBalance.Value -= business.LevelUpCost;

        // 2. Increment level
        business.Level++;

        // 3. Create a request to recalculate stats (income and next level cost)
        CreateRecalculateRequest(world, request.TargetBusiness);
    }

    private void CreateRecalculateRequest(EcsWorld world, EcsPackedEntity targetBusiness)
    {
        int recalcRequestEntity = world.NewEntity();
        ref var recalculateRequest = ref world.GetPool<RecalculateStatsRequest>().Add(recalcRequestEntity);
        recalculateRequest.TargetBusiness = targetBusiness;
    }

    private ref BalanceComponent GetPlayerBalance(EcsWorld world)
    {
        EcsFilter playerFilter = world.Filter<PlayerTag>().Inc<BalanceComponent>().End();
        int playerEntity = playerFilter.GetRawEntities()[0];
        return ref world.GetPool<BalanceComponent>().Get(playerEntity);
    }
}