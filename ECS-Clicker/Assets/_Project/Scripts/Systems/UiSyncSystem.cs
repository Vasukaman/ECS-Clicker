using Leopotam.EcsLite;
using UnityEngine.UI;
using TMPro;
using System.Linq;


/// <summary>
/// Updates all UI elements every frame to reflect the current game state.
/// </summary>
public class UiSyncSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {

        double playerBalance = GetPlayerBalance(systems.GetWorld());

        UpdatePlayerBalanceUI(systems, playerBalance);
        UpdateAllBusinessViews(systems, playerBalance);
    }

    /// <summary>
    /// Updates the main player balance text at the top of the screen.
    /// </summary>
    private void UpdatePlayerBalanceUI(EcsSystems systems, double playerBalance)
    {
        SceneData sceneData = systems.GetShared<SharedData>().SceneData;
        sceneData.BalanceText.text = $"{playerBalance:F0}$";
    }

    /// <summary>
    /// Loops through all businesses and updates their corresponding UI views.
    /// </summary>
    private void UpdateAllBusinessViews(EcsSystems systems, double playerBalance)
    {
        EcsWorld world = systems.GetWorld();
        GameConfig gameConfig = systems.GetShared<SharedData>().GameConfig;
        NamesConfig namesConfig = systems.GetShared<SharedData>().NamesConfig;

        EcsFilter businessFilter = world.Filter<BusinessComponent>().Inc<ViewComponent>().End();
        EcsPool<BusinessComponent> businessPool = world.GetPool<BusinessComponent>();
        EcsPool<ViewComponent> viewPool = world.GetPool<ViewComponent>();

        foreach (int entity in businessFilter)
        {
            ref BusinessComponent business = ref businessPool.Get(entity);
            ref ViewComponent view = ref viewPool.Get(entity);
            BusinessConfig config = gameConfig.Businesses[business.ConfigId];


            BusinessTextData namesTextData = namesConfig.AllBusinessTexts.FirstOrDefault(t => t.BusinessId == config.BusinessId);

            UpdateSingleBusinessView(ref business, ref view, config, namesTextData, playerBalance);
        }
    }

    /// <summary>
    /// Updates all the text and interactive elements for a single business panel.
    /// </summary>
    private void UpdateSingleBusinessView(ref BusinessComponent business, ref ViewComponent view, BusinessConfig config, BusinessTextData namesTextData, double playerBalance)
    {
        // Use the name from the combined text data
        view.Value.NameText.text = namesTextData.BusinessName;
        view.Value.LevelText.text = $"LVL\n{business.Level}";
        view.Value.IncomeText.text = $"{business.CurrentIncome:F0}$";

        view.Value.ProgressBar.value = business.IncomeTimer / config.IncomeDelay;

        view.Value.LevelUpButtonText.text = $"LVL UP\n{business.LevelUpCost:F0}$";
        view.Value.LevelUpButton.interactable = playerBalance >= business.LevelUpCost;

        // Pass the specific upgrade name down to the button helper
        UpdateUpgradeButton(view.Value.Upgrade1Button, view.Value.Upgrade1ButtonText, business.IsUpgrade1Purchased, config.Upgrade1, namesTextData.Upgrade1Name, playerBalance);
        UpdateUpgradeButton(view.Value.Upgrade2Button, view.Value.Upgrade2ButtonText, business.IsUpgrade2Purchased, config.Upgrade2, namesTextData.Upgrade2Name, playerBalance);
    }

    /// <summary>
    /// A generic helper to update the state of an upgrade button.
    /// </summary>
    private void UpdateUpgradeButton(Button button, TMP_Text buttonText, bool isPurchased, UpgradeConfig upgradeConfig, string upgradeName, double playerBalance)
    {
        if (isPurchased)
        {
            button.interactable = false;
            buttonText.text = $"{upgradeName}";
        }
        else
        {
            button.interactable = playerBalance >= upgradeConfig.Price;
            buttonText.text = $"{upgradeName}\n{upgradeConfig.Price:F0}$";
        }
    }

    private double GetPlayerBalance(EcsWorld world)
    {
        EcsFilter playerFilter = world.Filter<PlayerTag>().Inc<BalanceComponent>().End();

        if (playerFilter.GetEntitiesCount() > 0)
        {
            int playerEntity = playerFilter.GetRawEntities()[0];
            ref BalanceComponent balance = ref world.GetPool<BalanceComponent>().Get(playerEntity);
            return balance.Value;
        }

        return 0;
    }
}