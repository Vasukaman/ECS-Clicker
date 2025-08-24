// Filename: UpgradeSystem.cs
// Location: _Project/Scripts/ECS/Systems/
using Leopotam.EcsLite;

public class UpgradeSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        EcsFilter requestFilter = world.Filter<UpgradeRequest>().End();

        foreach (int requestEntity in requestFilter)
        {
            ProcessUpgradeRequest(systems, world, requestEntity);
            world.DelEntity(requestEntity);
        }
    }

    private void ProcessUpgradeRequest(EcsSystems systems, EcsWorld world, int requestEntity)
    {
        ref UpgradeRequest request = ref world.GetPool<UpgradeRequest>().Get(requestEntity);

        if (request.TargetBusiness.Unpack(world, out int businessEntity))
        {
            ref BusinessComponent business = ref world.GetPool<BusinessComponent>().Get(businessEntity);
            ref BalanceComponent playerBalance = ref GetPlayerBalance(world);

            if (CanPurchaseUpgrade(systems, ref business, ref playerBalance, ref request))
            {
                ApplyUpgrade(systems, world, ref playerBalance, ref business, ref request);
            }
        }
    }

    private bool CanPurchaseUpgrade(EcsSystems systems, ref BusinessComponent business, ref BalanceComponent playerBalance, ref UpgradeRequest request)
    {
        GameConfig gameConfig = systems.GetShared<SharedData>().GameConfig;
        BusinessConfig config = gameConfig.Businesses[business.ConfigId];
        UpgradeConfig upgradeConfig = (request.UpgradeId == 0) ? config.Upgrade1 : config.Upgrade2;
        bool isPurchased = (request.UpgradeId == 0) ? business.IsUpgrade1Purchased : business.IsUpgrade2Purchased;

        return !isPurchased && playerBalance.Value >= upgradeConfig.Price;
    }

    private void ApplyUpgrade(EcsSystems systems, EcsWorld world, ref BalanceComponent playerBalance, ref BusinessComponent business, ref UpgradeRequest request)
    {
        GameConfig gameConfig = systems.GetShared<SharedData>().GameConfig;
        BusinessConfig config = gameConfig.Businesses[business.ConfigId];
        UpgradeConfig upgradeConfig = (request.UpgradeId == 0) ? config.Upgrade1 : config.Upgrade2;

        playerBalance.Value -= upgradeConfig.Price;

        if (request.UpgradeId == 0)
            business.IsUpgrade1Purchased = true;
        else
            business.IsUpgrade2Purchased = true;

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