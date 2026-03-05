using Siege.Gameplay.Resources;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SpySellingIntelEvent : IGameEvent
    {
        const int MinDay = 10;
        const int CooldownDays = 7;
        const float TriggerChance = 0.15f;
        const double MaterialsCost = 10.0;
        const double FoodCost = 5.0;

        readonly ResourceLedger _ledger;
        int _lastTriggerDay = int.MinValue;

        public SpySellingIntelEvent(ResourceLedger ledger)
        {
            _ledger = ledger;
        }

        public string Id => "spy_selling_intel";
        public string Name => "Spy Selling Intel";
        public string Description => "A figure in a threadbare cloak approaches your guard captain, claiming to have intelligence on the enemy's plans \u2014 for a price.";

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse(
                "Buy Intel",
                $"Materials -{MaterialsCost}, Food -{FoodCost}. Siege intensity decreases.",
                "The spy's information is detailed and credible. Siege pressure eases."),
            new EventResponse("Turn Away", "Send the spy away empty-handed.")
        };

        public bool CanTrigger(GameState state)
        {
            if (state.CurrentDay < MinDay) return false;
            if ((state.CurrentDay - _lastTriggerDay) < CooldownDays) return false;
            if (Random.value >= TriggerChance) return false;
            _lastTriggerDay = state.CurrentDay;
            return true;
        }

        public void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            if (responseIndex == 0)
            {
                _ledger.Withdraw(ResourceType.Materials, MaterialsCost);
                _ledger.Withdraw(ResourceType.Food, FoodCost);
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                log.Record("Materials", -MaterialsCost, Name);
                log.Record("Food", -FoodCost, Name);
                log.Record("SiegeIntensity", -1, Name);
            }
        }

        public IGameEvent Clone() => new SpySellingIntelEvent(_ledger);
    }
}
