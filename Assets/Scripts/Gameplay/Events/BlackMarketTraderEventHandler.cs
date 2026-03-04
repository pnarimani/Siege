using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class BlackMarketTraderEventHandler : EventHandler<BlackMarketTraderEvent>
    {
        const int MinDay = 8;
        const int CooldownDays = 5;
        const float TriggerChance = 0.20f;
        const double MaterialsCostFull = 15.0;
        const double MaterialsCostHaggle = 8.0;
        const double FoodReward = 30.0;
        const double HaggleUnrestPenalty = 5.0;

        int _lastTriggerDay = int.MinValue;

        public BlackMarketTraderEventHandler(BlackMarketTraderEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= MinDay &&
            (state.CurrentDay - _lastTriggerDay) >= CooldownDays &&
            Random.value < TriggerChance;

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            _lastTriggerDay = state.CurrentDay;

            switch (responseIndex)
            {
                case 0:
                    state.AddResource(ResourceType.Materials, -MaterialsCostFull);
                    state.AddResource(ResourceType.Food, FoodReward);
                    log.Record("Materials", -MaterialsCostFull, Event.Name);
                    log.Record("Food", FoodReward, Event.Name);
                    break;
                case 1:
                    state.AddResource(ResourceType.Materials, -MaterialsCostHaggle);
                    state.AddResource(ResourceType.Food, FoodReward);
                    state.Unrest += HaggleUnrestPenalty;
                    log.Record("Materials", -MaterialsCostHaggle, Event.Name);
                    log.Record("Food", FoodReward, Event.Name);
                    log.Record("Unrest", HaggleUnrestPenalty, Event.Name);
                    break;
            }
        }
    }
}
