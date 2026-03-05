using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class BlackMarketTraderEvent : IGameEvent
    {
        const int MinDay = 8;
        const int CooldownDays = 5;
        const float TriggerChance = 0.20f;
        const double MaterialsCostFull = 15.0;
        const double MaterialsCostHaggle = 8.0;
        const double FoodReward = 30.0;
        const double HaggleUnrestPenalty = 5.0;

        int _lastTriggerDay = int.MinValue;

        public string Id => "black_market_trader";
        public string Name => "Black Market Trader";
        public string Description => "A shady figure offers a deal: materials for food.";

        public EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse("Accept", $"Give {MaterialsCostFull} Materials, receive {FoodReward} Food."),
            new EventResponse("Haggle", $"Give {MaterialsCostHaggle} Materials, receive {FoodReward} Food. Unrest +{HaggleUnrestPenalty}."),
            new EventResponse("Refuse", "Send the trader away.")
        };

        public string GetNarrativeText(GameState state) =>
            "A cloaked figure slips through the gate at dusk, offering a trade: " +
            "materials for food. The price is steep, but hunger makes beggars of us all.";

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
            switch (responseIndex)
            {
                case 0:
                    state.AddResource(ResourceType.Materials, -MaterialsCostFull);
                    state.AddResource(ResourceType.Food, FoodReward);
                    log.Record("Materials", -MaterialsCostFull, Name);
                    log.Record("Food", FoodReward, Name);
                    break;
                case 1:
                    state.AddResource(ResourceType.Materials, -MaterialsCostHaggle);
                    state.AddResource(ResourceType.Food, FoodReward);
                    state.Unrest += HaggleUnrestPenalty;
                    log.Record("Materials", -MaterialsCostHaggle, Name);
                    log.Record("Food", FoodReward, Name);
                    log.Record("Unrest", HaggleUnrestPenalty, Name);
                    break;
            }
        }

        public IGameEvent Clone() => new BlackMarketTraderEvent();
    }
}
