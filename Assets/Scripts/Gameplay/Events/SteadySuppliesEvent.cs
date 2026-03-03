using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SteadySuppliesEvent : GameEvent
    {
        const int StreakThreshold = 5;
        const double MaxMoraleBoost = 15.0;

        int _lastFiredDay = int.MinValue;

        public override string Id => "steady_supplies";
        public override string Name => "Steady Supplies";
        public override string Description => "Consistent supply flow boosts morale.";
        public override int Priority => 20;
        public override bool IsOneTime => false;

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveNoDeficitDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public override void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            double boost = Math.Min(MaxMoraleBoost, state.ConsecutiveNoDeficitDays);
            state.Morale += boost;
            log.Record("Morale", boost, Name);
        }

        public override string GetNarrativeText(GameState state) =>
            "The steady flow of food and water lifts spirits across the city.";
    }
}
