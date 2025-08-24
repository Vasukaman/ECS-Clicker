using Leopotam.EcsLite;
using UnityEngine;

/// <summary>
/// Updates the income progress timer for all active businesses each frame.
/// </summary>
public class IncomeProgressSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        EcsFilter filter = world.Filter<BusinessComponent>().End();
        EcsPool<BusinessComponent> businessPool = world.GetPool<BusinessComponent>();


        float deltaTime = Time.deltaTime;

        foreach (int entity in filter)
        {
            ref BusinessComponent business = ref businessPool.Get(entity);

            // Only update progress for businesses the player owns (Level > 0).
            if (business.Level > 0)
            {
                business.IncomeTimer += deltaTime;
            }
        }
    }
}