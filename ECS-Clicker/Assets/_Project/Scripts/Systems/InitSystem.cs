// Filename: InitSystem.cs
// Location: _Project/Scripts/Systems/
using Leopotam.EcsLite;
using UnityEngine; // Required for Object.Instantiate

public class InitSystem : IEcsInitSystem
{
    public void Init(EcsSystems systems)
    {
        var world = systems.GetWorld();
        var sharedData = systems.GetShared<SharedData>();
        var gameConfig = sharedData.GameConfig;
        var sceneData = sharedData.SceneData;

        // Create the Player entity
        var playerEntity = world.NewEntity();
        world.GetPool<PlayerTag>().Add(playerEntity);
        ref var balance = ref world.GetPool<BalanceComponent>().Add(playerEntity);
        balance.Value = gameConfig.StartingBalance;

        // Create entities for each business
        for (int i = 0; i < gameConfig.Businesses.Count; i++)
        {
            var businessEntity = world.NewEntity();
            ref var business = ref world.GetPool<BusinessComponent>().Add(businessEntity);
            var config = gameConfig.Businesses[i];

            // --- REFACTOR APPLIED ---
            // Use the array index 'i' as the ConfigId
            business.ConfigId = i;
            business.Level = (i == 0) ? 1 : 0;
            business.IncomeTimer = 0f;
            // We'll calculate CurrentIncome and LevelUpCost in a separate system/method
            // to avoid duplicating logic. For now, let's leave it simple.
            business.CurrentIncome = (business.Level > 0) ? config.BaseIncome : 0;
            business.LevelUpCost = (business.Level + 1) * config.BaseCost;


            // --- NEW LOGIC: SPAWN PREFAB AND INITIALIZE BUTTONS ---
            // 1. Instantiate the UI prefab
            BusinessView newView = Object.Instantiate(sceneData.BusinessViewPrefab, sceneData.BusinessPanelContainer);

            // 2. Link the entity to its newly created view
            ref var view = ref world.GetPool<ViewComponent>().Add(businessEntity);
            view.Value = newView;

            // 3. Find and initialize the button bridges on the new view
            EcsPackedEntity packedBusiness = world.PackEntity(businessEntity);
            var bridges = newView.GetComponentsInChildren<EcsClickEventBridge>();
            foreach (var bridge in bridges)
            {
                bridge.Initialize(world, packedBusiness);
            }
        }
    }
}