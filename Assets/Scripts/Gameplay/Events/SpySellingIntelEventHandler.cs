using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SpySellingIntelEventHandler : EventHandler<SpySellingIntelEvent>
    {
        const int MinDay = 10;
        const int CooldownDays = 7;
        const float TriggerChance = 0.15f;
        const double MaterialsCost = 10.0;
        const double FoodCost = 5.0;

        int _lastTriggerDay = int.MinValue;

        public SpySellingIntelEventHandler(SpySellingIntelEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= MinDay &&
            (state.CurrentDay - _lastTriggerDay) >= CooldownDays &&
            Random.value < TriggerChance;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            _lastTriggerDay = state.CurrentDay;

            if (responseIndex == 0)
            {
                state.AddResource(ResourceType.Materials, -MaterialsCost);
                state.AddResource(ResourceType.Food, -FoodCost);
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                log.Record("Materials", -MaterialsCost, Event.Name);
                log.Record("Food", -FoodCost, Event.Name);
                log.Record("SiegeIntensity", -1, Event.Name);
            }
        }
    }
}
