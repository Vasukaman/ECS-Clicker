using Leopotam.EcsLite;
using UnityEngine;

/// <summary>
/// A helper class responsible for gathering game state from the ECS world
/// and writing it to PlayerPrefs.
/// </summary>
public class SaveUtility
{
    private const string SaveVersionKey = "SaveVersion";
    private const int CurrentSaveVersion = 1;

    /// <summary>
    /// Saves the entire game state to PlayerPrefs.
    /// </summary>
    public void Save(EcsWorld world, GameConfig gameConfig)
    {
        SavePlayerData(world);
        SaveAllBusinessData(world, gameConfig);

        PlayerPrefs.SetInt(SaveVersionKey, CurrentSaveVersion);
       
        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

    /// <summary>
    /// Finds the player entity and saves its balance.
    /// </summary>
    private void SavePlayerData(EcsWorld world)
    {
        EcsFilter playerFilter = world.Filter<PlayerTag>().Inc<BalanceComponent>().End();
        EcsPool<BalanceComponent> balancePool = world.GetPool<BalanceComponent>();

        foreach (int entity in playerFilter)
        {
            ref BalanceComponent balance = ref balancePool.Get(entity);
            // TUTORIAL: PlayerPrefs doesn't have a 'SetDouble', so we save the balance as a string.
            PlayerPrefs.SetString("Player_Balance", balance.Value.ToString());
        }
    }

    /// <summary>
    /// Finds all business entities and saves their dynamic data.
    /// </summary>
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