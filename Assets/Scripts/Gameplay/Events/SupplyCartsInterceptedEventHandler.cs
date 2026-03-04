using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SupplyCartsInterceptedEventHandler : EventHandler<SupplyCartsInterceptedEvent>
    {
        public SupplyCartsInterceptedEventHandler(SupplyCartsInterceptedEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= 3 && state.CurrentDay <= 5 && Random.value < 0.20f;

        public override void Execute(GameState state, ChangeLog log)
        {
            Event.LostFood = Random.value < 0.5f;

            if (Event.LostFood)
            {
                state.AddResource(ResourceType.Food, -15);
                log.Record("Food", -15, Event.Name);
            }
            else
            {
                state.AddResource(ResourceType.Water, -15);
                log.Record("Water", -15, Event.Name);
            }
        }
    }
}
