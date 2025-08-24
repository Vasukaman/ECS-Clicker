// Filename: EcsClickEventBridge.cs
// Location: _Project/Scripts/Views/
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.UI;

public class EcsClickEventBridge : MonoBehaviour
{
    // Set this in the Inspector to determine the event type
    public enum ClickEventType { LevelUp, Upgrade1, Upgrade2 }
    [SerializeField] private ClickEventType _eventType;

    // We'll need access to the ECS World
    private EcsWorld _world;
    private EcsPackedEntity _targetBusiness;
    private Button _button;

    public void Initialize(EcsWorld world, EcsPackedEntity targetBusiness)
    {
        _world = world;
        _targetBusiness = targetBusiness;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(CreateRequestEntity);
    }

    private void CreateRequestEntity()
    {
        Debug.Log("Button clicked");

        var requestEntity = _world.NewEntity();

        if (_eventType == ClickEventType.LevelUp)
        {
            ref var request = ref _world.GetPool<LevelUpRequest>().Add(requestEntity);
            request.TargetBusiness = _targetBusiness;
        }
        else // It's an upgrade request
        {
            ref var request = ref _world.GetPool<UpgradeRequest>().Add(requestEntity);
            request.TargetBusiness = _targetBusiness;
            request.UpgradeId = (_eventType == ClickEventType.Upgrade1) ? 0 : 1;
        }
    }

    // Clean up the listener when the button is destroyed
    void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(CreateRequestEntity);
        }
    }
}