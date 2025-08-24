using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// A MonoBehaviour that sits on a UI Button and creates an ECS request entity when clicked.
/// Acts as a bridge between the UI and the ECS world.
/// </summary>
[RequireComponent(typeof(Button))]
public class EcsClickEventBridge : MonoBehaviour
{
    public enum ClickEventType { LevelUp, Upgrade1, Upgrade2 }

    [SerializeField] private ClickEventType _eventType;

    private EcsWorld _world;
    private EcsPackedEntity _targetBusiness;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    /// <summary>
    /// Initializes the bridge with the necessary ECS context. Called by the InitSystem.
    /// </summary>
    public void Initialize(EcsWorld world, EcsPackedEntity targetBusiness)
    {
        _world = world;
        _targetBusiness = targetBusiness;
        _button.onClick.AddListener(CreateRequestEntity);
    }

    /// <summary>
    /// Creates a new entity with the appropriate request component based on the event type set in the Inspector.
    /// </summary>
    private void CreateRequestEntity()
    {
        switch (_eventType)
        {
            case ClickEventType.LevelUp:
                CreateLevelUpRequest();
                break;
            case ClickEventType.Upgrade1:
                CreateUpgradeRequest(0);
                break;
            case ClickEventType.Upgrade2:
                CreateUpgradeRequest(1);
                break;
        }
    }

    private void CreateLevelUpRequest()
    {
        int requestEntity = _world.NewEntity();
        ref LevelUpRequest request = ref _world.GetPool<LevelUpRequest>().Add(requestEntity);
        request.TargetBusiness = _targetBusiness;
    }

    private void CreateUpgradeRequest(int upgradeId)
    {
        int requestEntity = _world.NewEntity();
        ref UpgradeRequest request = ref _world.GetPool<UpgradeRequest>().Add(requestEntity);
        request.TargetBusiness = _targetBusiness;
        request.UpgradeId = upgradeId;
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(CreateRequestEntity);
        }
    }
}