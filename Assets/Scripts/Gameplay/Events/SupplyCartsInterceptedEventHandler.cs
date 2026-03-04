using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SupplyCartsInterceptedEventHandler : IEventHandler
    {
        readonly SupplyCartsInterceptedEvent _event;

        public string EventId => _event.Id;

        public SupplyCartsInterceptedEventHandler(SupplyCartsInterceptedEvent gameEvent)
        {
            _event = gameEvent;
        }

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= 3 && state.CurrentDay <= 5 && Random.value < 0.20f;

        public void Execute(GameState state, ChangeLog log)
        {
            _event.LostFood = Random.value < 0.5f;

            if (_event.LostFood)
            {
                state.AddResource(ResourceType.Food, -15);
                log.Record("Food", -15, _event.Name);
            }
            else
            {
                state.AddResource(ResourceType.Water, -15);
                log.Record("Water", -15, _event.Name);
            }
        }
    }
}
