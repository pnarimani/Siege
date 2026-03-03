using Siege.Gameplay;
using Siege.Gameplay.Simulation;
using UnityEngine;

namespace Siege.Gameplay.Events
{
    public class SpySellingIntelEvent : GameEvent
    {
        const int MinDay = 10;
        const int CooldownDays = 7;
        const float TriggerChance = 0.15f;
        const double MaterialsCost = 10.0;
        const double FoodCost = 5.0;

        int _lastTriggerDay = int.MinValue;

        public override string Id => "spy_selling_intel";
        public override string Name => "Spy Selling Intel";
        public override string Description => "A spy offers intelligence on enemy movements.";
        public override int Priority => 50;
        public override bool IsOneTime => false;
        public override bool IsRespondable => true;

        public override bool CanTrigger(GameState state) =>
            state.CurrentDay >= MinDay &&
            (state.CurrentDay - _lastTriggerDay) >= CooldownDays &&
            Random.value < TriggerChance;

        public override EventResponse[] GetResponses(GameState state) => new[]
        {
            new EventResponse(
                "Buy Intel",
                $"Materials -{MaterialsCost}, Food -{FoodCost}. Siege intensity decreases.",
                "The spy's information is detailed and credible. Siege pressure eases."),
            new EventResponse("Turn Away", "Send the spy away empty-handed.")
        };

        public override void ExecuteResponse(GameState state, ChangeLog log, int responseIndex)
        {
            _lastTriggerDay = state.CurrentDay;

            if (responseIndex == 0)
            {
                state.AddResource(ResourceType.Materials, -MaterialsCost);
                state.AddResource(ResourceType.Food, -FoodCost);
                state.SiegeIntensity = System.Math.Max(1, state.SiegeIntensity - 1);
                log.Record("Materials", -MaterialsCost, Name);
                log.Record("Food", -FoodCost, Name);
                log.Record("SiegeIntensity", -1, Name);
            }
        }

        public override string GetNarrativeText(GameState state) =>
            "A figure in a threadbare cloak approaches your guard captain, " +
            "claiming to have intelligence on the enemy's plans — for a price.";
    }
}
