// Filename: SaveUtility.cs
// Location: _Project/Scripts/Services/ (or a new /Persistence/ folder)
using Leopotam.EcsLite;
using UnityEngine;

public class SaveUtility
{
    private const string SaveVersionKey = "SaveVersion";
    private const int CurrentSaveVersion = 1;

    public void Save(EcsWorld world, GameConfig gameConfig)
    {
        SavePlayerData(world);
        SaveAllBusinessData(world, gameConfig);

        // Finalize the save by writing to disk
        PlayerPrefs.SetInt(SaveVersionKey, CurrentSaveVersion);
        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

    private void SavePlayerData(EcsWorld world)
    {
        EcsFilter playerFilter = world.Filter<PlayerTag>().Inc<BalanceComponent>().End();
        EcsPool<BalanceComponent> balancePool = world.GetPool<BalanceComponent>();

        foreach (int entity in playerFilter)
        {
            // Get a reference to the balance component to read its value
            ref BalanceComponent balance = ref balancePool.Get(entity);
            PlayerPrefs.SetString("Player_Balance", balance.Value.ToString());
        }
    }

    private void SaveAllBusinessData(EcsWorld world, GameConfig gameConfig)
    {
        EcsFilter businessFilter = world.Filter<BusinessComponent>().End();
        EcsPool<BusinessComponent> businessPool = world.GetPool<BusinessComponent>();

        foreach (int entity in businessFilter)
        {
            ref BusinessComponent business = ref businessPool.Get(entity);
            BusinessConfig businessConfig = gameConfig.Businesses[business.ConfigId];
            string id = businessConfig.BusinessId;

            PlayerPrefs.SetInt($"{id}_Level", business.Level);
            PlayerPrefs.SetFloat($"{id}_IncomeTimer", business.IncomeTimer);
            PlayerPrefs.SetInt($"{id}_Upgrade1", business.IsUpgrade1Purchased ? 1 : 0);
            PlayerPrefs.SetInt($"{id}_Upgrade2", business.IsUpgrade2Purchased ? 1 : 0);
        }
    }
}