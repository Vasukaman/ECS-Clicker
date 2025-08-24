using Leopotam.EcsLite;
using System;

/// <summary>
/// Processes player requests to purchase upgrades for businesses.
/// </summary>
public class UpgradeSystem : IEcsRunSystem
{
    /// <summary>
    /// The main entry point for the system, called every frame by EcsSystems.
    /// </summary>
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

    /// <summary>
    /// Processes a single upgrade request, checking for validity and applying the upgrade if possible.
    /// </summary>
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

    /// <summary>
    /// Checks if an upgrade can be purchased (is not already owned and the player has enough money).
    /// </summary>
    private bool CanPurchaseUpgrade(EcsSystems systems, ref BusinessComponent business, ref BalanceComponent playerBalance, ref UpgradeRequest request)
    {
        GameConfig gameConfig = systems.GetShared<SharedData>().GameConfig;
        BusinessConfig config = gameConfig.Businesses[business.ConfigId];
        UpgradeConfig upgradeConfig = (request.UpgradeId == 0) ? config.Upgrade1 : config.Upgrade2;
        bool isPurchased = (request.UpgradeId == 0) ? business.IsUpgrade1Purchased : business.IsUpgrade2Purchased;

        return !isPurchased && playerBalance.Value >= upgradeConfig.Price;
    }

    /// <summary>
    /// Applies the upgrade effects: deducts cost, marks the business as upgraded, and fires a recalculation event.
    /// </summary>
    private void ApplyUpgrade(EcsSystems systems, EcsWorld world, ref BalanceComponent playerBalance, ref BusinessComponent business, ref UpgradeRequest request)
    {
        GameConfig gameConfig = systems.GetShared<SharedData>().GameConfig;
        BusinessConfig config = gameConfig.Businesses[business.ConfigId];
        UpgradeConfig upgradeConfig = (request.UpgradeId == 0) ? config.Upgrade1 : config.Upgrade2;

        // 1. Deduct cost from the player's balance.
        playerBalance.Value -= upgradeConfig.Price;

        // 2. Mark the upgrade as purchased on the business component.
        if (request.UpgradeId == 0)
            business.IsUpgrade1Purchased = true;
        else
            business.IsUpgrade2Purchased = true;


        CreateRecalculateRequest(world, request.TargetBusiness);
    }

    /// <summary>
    /// Helper method to create a RecalculateStatsRequest event for a target business.
    /// </summary>
    private void CreateRecalculateRequest(EcsWorld world, EcsPackedEntity targetBusiness)
    {
        int recalcRequestEntity = world.NewEntity();
        ref RecalculateStatsRequest recalculateRequest = ref world.GetPool<RecalculateStatsRequest>().Add(recalcRequestEntity);
        recalculateRequest.TargetBusiness = targetBusiness;
    }

    /// <summary>
    /// A safe way to get a reference to the single player's BalanceComponent.
    /// </summary>
    private ref BalanceComponent GetPlayerBalance(EcsWorld world)
    {
        EcsFilter playerFilter = world.Filter<PlayerTag>().Inc<BalanceComponent>().End();
        if (playerFilter.GetEntitiesCount() == 0)
        {

            throw new Exception("Player entity with BalanceComponent not found!");
        }

        int playerEntity = playerFilter.GetRawEntities()[0];
        return ref world.GetPool<BalanceComponent>().Get(playerEntity);
    }
}