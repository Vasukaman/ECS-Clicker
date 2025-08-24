using Leopotam.EcsLite;

/// <summary>
/// Listens for revenue collection events and triggers visual feedback (like animations).
/// </summary>
public class UIPayoutFeedbackSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        EcsFilter eventFilter = world.Filter<RevenueCollectedEvent>().End();
        EcsPool<RevenueCollectedEvent> eventPool = world.GetPool<RevenueCollectedEvent>();
        EcsPool<ViewComponent> viewPool = world.GetPool<ViewComponent>();

        foreach (int eventEntity in eventFilter)
        {
            ref RevenueCollectedEvent evt = ref eventPool.Get(eventEntity);

            // Unpack the source business from the event
            if (evt.SourceBusiness.Unpack(world, out int businessEntity))
            {
                // Get the ViewComponent for that business
                ref ViewComponent view = ref viewPool.Get(businessEntity);

                // If the view has an animation controller, play the animation
                if (view.Value.PanelWobbleAnimation != null)
                {
                    view.Value.PanelWobbleAnimation.PlayWobble();
                }
            }
        }
    }
}