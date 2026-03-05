using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SupplyCartsInterceptedEvent : IGameEvent
    {
        bool _lostFood;

        public string Id => "supply_carts_intercepted";
        public string Name => "Supply Carts Intercepted";
        public string Description => "Enemy raiders have intercepted supply carts bound for the city.";

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= 3 && state.CurrentDay <= 5 && Random.value < 0.20f;

        public void Execute(GameState state, ChangeLog log)
        {
            _lostFood = Random.value < 0.5f;

            if (_lostFood)
            {
                state.AddResource(ResourceType.Food, -15);
                log.Record("Food", -15, Name);
            }
            else
            {
                state.AddResource(ResourceType.Water, -15);
                log.Record("Water", -15, Name);
            }
        }

        public string GetNarrativeText(GameState state) =>
            _lostFood
                ? "Enemy raiders intercepted a food convoy. Precious grain lost to the siege lines."
                : "Enemy raiders seized water barrels en route to the city. Our reserves shrink.";

        public IGameEvent Clone() => new SupplyCartsInterceptedEvent();
    }
}
