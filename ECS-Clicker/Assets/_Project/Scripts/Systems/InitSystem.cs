// In InitSystem.cs
using Leopotam.EcsLite;
using UnityEngine;

public class InitSystem : IEcsInitSystem
{
    public void Init(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        GameConfig gameConfig = systems.GetShared<SharedData>().GameConfig;
        SceneData sceneData = systems.GetShared<SharedData>().SceneData;
        bool hasSave = PlayerPrefs.HasKey("SaveVersion");

        InitializePlayer(world, gameConfig, hasSave);

        for (int i = 0; i < gameConfig.Businesses.Count; i++)
        {
            InitializeBusiness(world, gameConfig, sceneData, i, hasSave);
        }
    }

    private void InitializePlayer(EcsWorld world, GameConfig gameConfig, bool hasSave)
    {
        int playerEntity = world.NewEntity();
        world.GetPool<PlayerTag>().Add(playerEntity);
        ref BalanceComponent balance = ref world.GetPool<BalanceComponent>().Add(playerEntity);
        balance.Value = GetInitialBalance(hasSave, gameConfig);
    }

    private void InitializeBusiness(EcsWorld world, GameConfig gameConfig, SceneData sceneData, int index, bool hasSave)
    {
        int businessEntity = world.NewEntity();
        ref BusinessComponent business = ref world.GetPool<BusinessComponent>().Add(businessEntity);
        business = GetInitialBusinessState(hasSave, gameConfig.Businesses[index], index);

        CreateBusinessView(world, businessEntity, sceneData);
        CreateRecalculateRequest(world, world.PackEntity(businessEntity));
    }

    private void CreateBusinessView(EcsWorld world, int businessEntity, SceneData sceneData)
    {
        BusinessView newView = Object.Instantiate(sceneData.BusinessViewPrefab, sceneData.BusinessPanelContainer);
        ref ViewComponent view = ref world.GetPool<ViewComponent>().Add(businessEntity);
        view.Value = newView;

        EcsPackedEntity packedBusiness = world.PackEntity(businessEntity);
        EcsClickEventBridge[] bridges = newView.GetComponentsInChildren<EcsClickEventBridge>();
        foreach (EcsClickEventBridge bridge in bridges)
        {
            bridge.Initialize(world, packedBusiness);
        }
    }

    private double GetInitialBalance(bool hasSave, GameConfig gameConfig)
    {
        if (!hasSave) return gameConfig.StartingBalance;

        string savedBalance = PlayerPrefs.GetString("Player_Balance", "0");
        double.TryParse(savedBalance, out double result);
        return result;
    }

    private void CreateRecalculateRequest(EcsWorld world, EcsPackedEntity targetBusiness)
    {
        int requestEntity = world.NewEntity();
        ref RecalculateStatsRequest request = ref world.GetPool<RecalculateStatsRequest>().Add(requestEntity);
        request.TargetBusiness = targetBusiness;
    }


    private BusinessComponent GetInitialBusinessState(bool hasSave, BusinessConfig config, int index)
    {
        BusinessComponent businessState = new BusinessComponent();
        businessState.ConfigId = index;

        if (hasSave)
        {
            businessState.Level = PlayerPrefs.GetInt($"{config.BusinessId}_Level", config.InitialLevel);
            businessState.IncomeTimer = PlayerPrefs.GetFloat($"{config.BusinessId}_IncomeTimer", 0f);
            businessState.IsUpgrade1Purchased = PlayerPrefs.GetInt($"{config.BusinessId}_Upgrade1", 0) == 1;
            businessState.IsUpgrade2Purchased = PlayerPrefs.GetInt($"{config.BusinessId}_Upgrade2", 0) == 1;
        }
        else
        {
            businessState.Level = config.InitialLevel; // Use the value from the SO
            businessState.IncomeTimer = 0f;
            businessState.IsUpgrade1Purchased = false;
            businessState.IsUpgrade2Purchased = false;
        }
        return businessState;
    }
}