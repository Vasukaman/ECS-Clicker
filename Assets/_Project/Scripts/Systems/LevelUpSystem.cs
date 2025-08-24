using Leopotam.EcsLite;
using System;

/// <summary>
/// Processes player requests to level up (or purchase for the first time) a business.
/// </summary>
public class LevelUpSystem : IEcsRunSystem
{
    /// <summary>
    /// The main entry point for the system, called every frame.
    /// </summary>
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        EcsFilter requestFilter = world.Filter<LevelUpRequest>().End();

        foreach (int requestEntity in requestFilter)
        {
            ProcessLevelUpRequest(world, requestEntity);
            world.DelEntity(requestEntity);
        }
    }

    /// <summary>
    /// Processes a single level up request by checking affordability and applying the level up.
    /// </summary>
    private void ProcessLevelUpRequest(EcsWorld world, int requestEntity)
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

    /// <summary>
    /// Checks if the player has enough money to afford the next level.
    /// </summary>
    private bool CanAffordLevelUp(ref BusinessComponent business, ref BalanceComponent playerBalance)
    {
        return playerBalance.Value >= business.LevelUpCost;
    }

    /// <summary>
    /// Applies the level up effects: deducts cost, increments the level, and fires a recalculation event.
    /// </summary>
    private void ApplyLevelUp(EcsWorld world, ref BalanceComponent playerBalance, ref BusinessComponent business, ref LevelUpRequest request)
    {
        // 1. Deduct cost from the player's balance.
        playerBalance.Value -= business.LevelUpCost;

        // 2. Increment the business's level.
        business.Level++;

        // 3. Create an event to signal that this business's stats need to be updated.
        CreateRecalculateRequest(world, request.TargetBusiness);
    }

    /// <summary>
    /// Helper method to create a RecalculateStatsRequest event.
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