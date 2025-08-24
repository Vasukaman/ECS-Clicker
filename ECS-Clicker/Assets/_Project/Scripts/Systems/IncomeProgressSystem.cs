// Filename: IncomeProgressSystem.cs
// Location: _Project/Scripts/ECS/Systems/
using Leopotam.EcsLite;
using UnityEngine;

public class IncomeProgressSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<BusinessComponent>().End();
        var businessPool = world.GetPool<BusinessComponent>();

        foreach (var entity in filter)
        {
            ref var business = ref businessPool.Get(entity);

            // Only update progress for businesses the player owns
            if (business.Level > 0)
            {
                business.IncomeTimer += Time.deltaTime;
            }
        }
    }
}