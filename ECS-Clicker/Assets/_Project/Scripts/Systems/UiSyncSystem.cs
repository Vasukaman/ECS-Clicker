// Filename: UiSyncSystem.cs
// Location: _Project/Scripts/Systems/
using Leopotam.EcsLite;
using UnityEngine.UI; // For making buttons interactable
using TMPro; // For TextMeshPro

public class UiSyncSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        var world = systems.GetWorld();
        var sharedData = systems.GetShared<SharedData>();
        var gameConfig = sharedData.GameConfig;
        var sceneData = sharedData.SceneData;

        // --- Player Balance ---
        var playerFilter = world.Filter<PlayerTag>().Inc<BalanceComponent>().End();
        var balancePool = world.GetPool<BalanceComponent>();

        double playerBalanceValue = 0; // Get player balance once to use in the loop below
        foreach (var entity in playerFilter)
        {
            ref var balance = ref balancePool.Get(entity);
            playerBalanceValue = balance.Value;
            sceneData.BalanceText.text = $"{playerBalanceValue:F0}$";
        }

        // --- Business UI ---
        var businessFilter = world.Filter<BusinessComponent>().Inc<ViewComponent>().End();
        var businessPool = world.GetPool<BusinessComponent>();
        var viewPool = world.GetPool<ViewComponent>();

        foreach (var entity in businessFilter)
        {
            ref var business = ref businessPool.Get(entity);
            ref var view = ref viewPool.Get(entity);
            var config = gameConfig.Businesses[business.ConfigId];

            // --- READ pre-calculated values, DON'T recalculate here ---
            view.Value.NameText.text = config.BusinessName;
            view.Value.LevelText.text = $"LVL\n{business.Level}";
            view.Value.IncomeText.text = $"{business.CurrentIncome:F0}$";
            view.Value.LevelUpButtonText.text = $"LVL UP\n{business.LevelUpCost:F0}$";

            // --- Update dynamic UI elements that change every frame ---
            view.Value.ProgressBar.value = business.IncomeTimer / config.IncomeDelay;

            // --- Update Button States based on affordability and purchase status ---
            view.Value.LevelUpButton.interactable = playerBalanceValue >= business.LevelUpCost;

            // Update Upgrade 1 Button
            if (business.IsUpgrade1Purchased)
            {
                view.Value.Upgrade1Button.interactable = false;
                view.Value.Upgrade1ButtonText.text = "Purchased";
            }
            else
            {
                view.Value.Upgrade1Button.interactable = playerBalanceValue >= config.Upgrade1.Price;
                view.Value.Upgrade1ButtonText.text = $"{config.Upgrade1.Name}\n{config.Upgrade1.Price:F0}$";
            }

            // Update Upgrade 2 Button
            if (business.IsUpgrade2Purchased)
            {
                view.Value.Upgrade2Button.interactable = false;
                view.Value.Upgrade2ButtonText.text = "Purchased";
            }
            else
            {
                view.Value.Upgrade2Button.interactable = playerBalanceValue >= config.Upgrade2.Price;
                view.Value.Upgrade2ButtonText.text = $"{config.Upgrade2.Name}\n{config.Upgrade2.Price:F0}$";
            }
        }
    }
}