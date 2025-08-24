// Filename: EcsStartup.cs
// Location: _Project/Scripts/
using Leopotam.EcsLite;
using UnityEngine;

// Define the container for shared data

public class EcsStartup : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private SceneData _sceneData; // Holds references to UI elements, etc.

    private EcsWorld _world;
    private EcsSystems _systems;

    void Start()
    {
        _world = new EcsWorld();

        // 1. Create an instance of your shared data container
        var sharedData = new SharedData
        {
            GameConfig = _gameConfig,
            SceneData = _sceneData
        };

        // 2. Pass the world AND the shared data to the EcsSystems constructor
        _systems = new EcsSystems(_world, sharedData);

        // 3. Add your systems as before
        _systems
              .Add(new InitSystem())
              // Gameplay Logic
              .Add(new IncomeProgressSystem()) // Runs first to update timers
              .Add(new BusinessRevenueSystem()) // Creates events
              .Add(new UpgradeSystem())
              .Add(new LevelUpSystem())
              .Add(new PlayerBalanceSystem())
              .Add(new BusinessStatsSystem())
              .Add(new UiSyncSystem())
              .Add(new EventCleanupSystem());

        _systems.Init();
    }

    void Update()
    {
        _systems?.Run();
    }

    void OnDestroy()
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }
        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }
}