using Leopotam.EcsLite;
using UnityEngine;
using System.Linq;

/// <summary>
/// The main entry point for the game's ECS world.
/// This MonoBehaviour creates the EcsWorld and EcsSystems, and manages their lifecycle.
/// </summary>
public class EcsStartup : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private SceneData _sceneData;
    [SerializeField] private NamesConfig _namesConfig;

    private EcsWorld _world;
    private EcsSystems _systems;
    private SaveUtility _saveUtility;


    private void Awake()
    {
        _world = new EcsWorld();
        _saveUtility = new SaveUtility();
    }


    private void Start()
    {
        SharedData sharedData = new SharedData
        {
            GameConfig = _gameConfig,
            NamesConfig = _namesConfig,
            SceneData = _sceneData
        };

        _systems = new EcsSystems(_world, sharedData);
        _systems
            // 1. Initialization
            .Add(new InitSystem())

            // 2. Game Logic (progress timers, check for payouts)
            .Add(new IncomeProgressSystem())
            .Add(new BusinessPayoutSystem())

            // 3. Player Action Handlers (react to clicks)
            .Add(new LevelUpSystem())
            .Add(new UpgradeSystem())

            // 4. State Update Systems (react to other systems' events)
            .Add(new PlayerBalanceSystem())
            .Add(new RecalculateStatsSystem())

            // 5. Presentation & Cleanup (update UI, destroy temporary events)
            .Add(new UiSyncSystem())
            .Add(new UIPayoutFeedbackSystem())
            .Add(new EventCleanupSystem());

        _systems.Init();
    }

    private void Update()
    {
        _systems?.Run();
    }

    private void OnDestroy()
    {
        if (_world != null)
        {
            _saveUtility.Save(_world, _gameConfig);
        }

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