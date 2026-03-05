using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SupplyCartsInterceptedEvent : IGameEvent
    {
        readonly ResourceLedger _ledger;
        bool _lostFood;

        public SupplyCartsInterceptedEvent(ResourceLedger ledger)
        {
            _ledger = ledger;
        }

        public string Id => "supply_carts_intercepted";
        public string Name => "Supply Carts Intercepted";
        public string Description => _lostFood
            ? "Enemy raiders intercepted a food convoy. Precious grain lost to the siege lines."
            : "Enemy raiders seized water barrels en route to the city. Our reserves shrink.";

        public bool CanTrigger(GameState state) =>
            state.CurrentDay >= 3 && state.CurrentDay <= 5 && Random.value < 0.20f;

        public void Execute(GameState state, ChangeLog log)
        {
            _lostFood = Random.value < 0.5f;

            if (_lostFood)
            {
                _ledger.Withdraw(ResourceType.Food, 15);
                log.Record("Food", -15, Name);
            }
            else
            {
                _ledger.Withdraw(ResourceType.Water, 15);
                log.Record("Water", -15, Name);
            }
        }

        public IGameEvent Clone() => new SupplyCartsInterceptedEvent(_ledger);
    }
}
