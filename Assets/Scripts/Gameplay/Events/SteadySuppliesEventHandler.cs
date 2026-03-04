using System;
using Siege.Gameplay.Simulation;

namespace Siege.Gameplay.Events
{
    public class SteadySuppliesEventHandler : EventHandler<SteadySuppliesEvent>
    {
        const int StreakThreshold = 5;
        const double MaxMoraleBoost = 15.0;

        int _lastFiredDay = int.MinValue;

        public SteadySuppliesEventHandler(SteadySuppliesEvent gameEvent) : base(gameEvent) { }

        public override bool CanTrigger(GameState state) =>
            state.ConsecutiveNoDeficitDays >= StreakThreshold &&
            state.CurrentDay != _lastFiredDay;

        public override void Execute(GameState state, ChangeLog log)
        {
            _lastFiredDay = state.CurrentDay;
            double boost = Math.Min(MaxMoraleBoost, state.ConsecutiveNoDeficitDays);
            state.Morale += boost;
            log.Record("Morale", boost, Event.Name);
        }
    }
}
